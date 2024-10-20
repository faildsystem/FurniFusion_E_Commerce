using FurniFusion.Data;
using FurniFusion.Dtos.Order;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Transactions;

namespace FurniFusion.Services
{
    public class OrderService : IOrderService
    {

        private readonly FurniFusionDbContext _context;
        private readonly IPaymentService _paymentService;

        public OrderService(FurniFusionDbContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        public async Task<ServiceResult<Order>> ApplyDiscountToOrderAsync(int orderId, int discountId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return ServiceResult<Order>.ErrorResult($"Order with ID {orderId} not found.", StatusCodes.Status404NotFound);
            }

            var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == discountId);

            if (discount == null)
            {
                return ServiceResult<Order>.ErrorResult($"Discount with ID {discountId} not found.", StatusCodes.Status404NotFound);
            }

            if (discount.IsActive == false || discount.ValidFrom > DateOnly.FromDateTime(DateTime.Now) || discount.ValidTo < DateOnly.FromDateTime(DateTime.Now))
            {
                return ServiceResult<Order>.ErrorResult("Discount is not active or valid", StatusCodes.Status400BadRequest);
            }

            order.DiscountId = discount.DiscountId;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();

            return ServiceResult<Order>.SuccessResult(order, "Order applied to product successfully", StatusCodes.Status200OK);
        }

        public decimal CalculateOrderItemTotalPrice(
    string discountUnit, decimal? discountValue, int? quantity, decimal unitPrice,
    DateOnly? validFrom, DateOnly? validTo, bool? isActive, decimal? maxAmount)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            if (!isActive.HasValue || !isActive.Value)
                return unitPrice * (quantity ?? 0);

            if (!validFrom.HasValue || !validTo.HasValue)
                return unitPrice * (quantity ?? 0);

            if (validFrom > today || validTo < today)
            {
                return unitPrice * (quantity ?? 0);
            }

            decimal totalPrice = unitPrice * (quantity ?? 0);
            decimal totalDiscount = 0;

            if (discountUnit.Equals("PERCENT", StringComparison.OrdinalIgnoreCase) && discountValue.HasValue)
            {
                totalDiscount = totalPrice * (discountValue.Value / 100);
            }
            else if (discountUnit.Equals("AMOUNT", StringComparison.OrdinalIgnoreCase) && discountValue.HasValue)
            {
                totalDiscount = (quantity ?? 0) * discountValue.Value;
            }
            else
            {
                // Optionally log unexpected discountUnit
                throw new ArgumentException($"Unexpected discount unit: {discountUnit}");
            }

            if (maxAmount.HasValue && totalDiscount > maxAmount.Value)
            {
                totalDiscount = maxAmount.Value;
            }

            decimal finalPrice = totalPrice - totalDiscount;
            return finalPrice >= 0 ? finalPrice : 0;
        }

        public async Task<ServiceResult<bool>> CancelOrderAsync(int orderId, string userId)
        {
            var order = await _context.Orders
                .Include(o => o.Payment)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return ServiceResult<bool>.ErrorResult("Order not found", StatusCodes.Status404NotFound);

            if (order.UserId != userId)
                return ServiceResult<bool>.ErrorResult("Unauthorized", StatusCodes.Status403Forbidden);

            if (order.Status == 4)
                return ServiceResult<bool>.ErrorResult("Order already canceled", StatusCodes.Status400BadRequest);

            if (order.Status != 1)
                return ServiceResult<bool>.ErrorResult("Order cannot be canceled", StatusCodes.Status400BadRequest);

            if (order.Payment == null)
                return ServiceResult<bool>.ErrorResult("Payment not found", StatusCodes.Status404NotFound);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    order.Status = 4;
                    order.UpdatedAt = DateTime.Now;
                    order.Payment.PaymentStatusId = 3;

                    foreach (var item in order.OrderItems)
                    {
                        var product = await _context.Products
                            .FirstOrDefaultAsync(p => p.ProductId == item.ProductId);

                        if (product == null)
                            continue;

                        product.StockQuantity += item.Quantity;

                        if (product.IsAvailable.HasValue && !product.IsAvailable.Value && product.StockQuantity > 0)
                            product.IsAvailable = true;
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Optionally log the exception
                    // Logger.Error(ex, "Error canceling order.");
                    return ServiceResult<bool>.ErrorResult(ex.Message, StatusCodes.Status500InternalServerError);
                }
            }

            return ServiceResult<bool>.SuccessResult(true, "Order canceled successfully", StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<UpdateOrdersStatusDto?>> ChangeOrderStatusAsync(Dictionary<int, int> data)
        {
            var updateOrdersStatusDto = new UpdateOrdersStatusDto();

            var orderIds = data.Keys.ToList();
            var orders = await _context.Orders
                                       .Where(o => orderIds.Contains(o.OrderId))
                                       .ToListAsync();

            var existingOrderIds = orders.Select(o => o.OrderId).ToHashSet();
            var failedOrders = new HashSet<int>();

            foreach (var orderId in orderIds)
            {
                if (!existingOrderIds.Contains(orderId))
                {
                    failedOrders.Add(orderId);
                    continue;
                }

                var order = orders.First(o => o.OrderId == orderId);
                var newStatus = data[orderId];

                if (!IsValidStatusTransition((int)order.Status!, newStatus))
                {
                    failedOrders.Add(orderId);
                    continue;
                }

                order.Status = newStatus;
                order.UpdatedAt = DateTime.Now;
                updateOrdersStatusDto.UpdatedOrders.Add(orderId);
            }

            if (updateOrdersStatusDto.UpdatedOrders.Any())
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Optionally log the exception
                    // Logger.Error(ex, "Concurrency error while changing order statuses.");
                    return ServiceResult<UpdateOrdersStatusDto?>.ErrorResult("A concurrency error occurred.", StatusCodes.Status500InternalServerError);
                }
            }

            updateOrdersStatusDto.FailedOrders.AddRange(failedOrders);

            if (updateOrdersStatusDto.UpdatedOrders.Any())
            {
                return ServiceResult<UpdateOrdersStatusDto?>.SuccessResult(
                    updateOrdersStatusDto,
                    "Orders updated successfully",
                    StatusCodes.Status200OK);
            }
            else
            {
                return ServiceResult<UpdateOrdersStatusDto?>.ErrorResult(
                    "No orders were updated. Check failed orders for details.",
                    StatusCodes.Status400BadRequest);
            }
        }

        public async Task<ServiceResult<bool>> CreateOrderAsync(CreateOrderDto createOrderDto, string userId)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return ServiceResult<bool>.ErrorResult("Cart not found", StatusCodes.Status404NotFound);

            var cartItems = await _context.ShoppingCartItems.Where(c => c.CartId == cart.CartId).Select(c => new { c.ProductId, c.Quantity }).ToListAsync();

            if (cartItems.Count == 0)
                return ServiceResult<bool>.ErrorResult("Cart is empty", StatusCodes.Status400BadRequest);

            var productsIds = cartItems.Select(c => c.ProductId).ToList();

            var cartProducts = await _context.Products
                                        .Include(p => p.Discount)
                                        .ThenInclude(d => d!.DiscountUnit)
                                        .Where(p => productsIds.Contains(p.ProductId))
                                         .ToListAsync();


            var missingProductIds = productsIds.Except(cartProducts.Select(p => p.ProductId)).ToList();

            if (missingProductIds.Any())
                return ServiceResult<bool>.ErrorResult("Some products in the cart are no longer available.", StatusCodes.Status400BadRequest);

            var orderItems = new List<OrderItem>();
            decimal totalPrice = 0;

            foreach (var cartItem in cartItems)
            {
                var product = cartProducts.First(p => p.ProductId == cartItem.ProductId);

                if ((bool)!product.IsAvailable!)
                    return ServiceResult<bool>.ErrorResult($"Product {product.ProductName} is not available", StatusCodes.Status400BadRequest);

                if (cartItem.Quantity < 1)
                    return ServiceResult<bool>.ErrorResult($"Invalid quantity for product {product.ProductName}", StatusCodes.Status400BadRequest);

                if (product.StockQuantity < cartItem.Quantity)
                    return ServiceResult<bool>.ErrorResult($"Not enough stock for product {product.ProductName}", StatusCodes.Status400BadRequest);


                decimal discountedPrice = CalculateOrderItemTotalPrice(
                        product.Discount?.DiscountUnit?.UnitName ?? string.Empty, // Safely handle null Discount and DiscountUnit
                        product.Discount?.DiscountValue ?? 0, // Default to 0 if DiscountValue is null
                        cartItem.Quantity,
                        product.Price,
                        product.Discount?.ValidFrom, // Pass null if ValidFrom is null
                        product.Discount?.ValidTo,   // Pass null if ValidTo is null
                        product.Discount?.IsActive ?? false, // Default to false if IsActive is null
                        product.Discount?.MaxAmount // Pass null if MaxAmount is null
                    );


                totalPrice += discountedPrice;

                orderItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = (int)cartItem.Quantity!,
                    Price = discountedPrice, // Assuming this is total price for the item
                    CreatedAt = DateTime.Now,
                });

                // Update product stock in-memory
                product.StockQuantity -= cartItem.Quantity;
                if (product.StockQuantity == 0)
                    product.IsAvailable = false;
            }

            var payment = new Payment
            {
                Amount = totalPrice,
                UserId = userId,
                Date = DateTime.Now.Date,
                PaymentStatusId = 1,
                PaymentMethod = 1,
                TransactionId = Guid.NewGuid().ToString(),
            };

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    var paymentId = await _paymentService.DoPaymentAsync(payment);

                    if (paymentId == null)
                        return ServiceResult<bool>.ErrorResult("Payment failed", StatusCodes.Status500InternalServerError);

                    var order = new Order
                    {
                        UserId = userId,
                        OrderItems = orderItems,
                        AddressToDeliver = createOrderDto.AddressToDeliver,
                        PaymentId = paymentId,
                        TotalPrice = totalPrice,
                        Status = 1,
                        ShippingId = 1,
                        DiscountId = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    var createdOrder = await _context.Orders.AddAsync(order);

                    await _context.ShoppingCartItems.Where(c => c.CartId == cart.CartId).ExecuteDeleteAsync();

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    return ServiceResult<bool>.ErrorResult(ex.Message, StatusCodes.Status500InternalServerError);
                }

            }



            return ServiceResult<bool>.SuccessResult(true, "Order created successfully", StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<Order?>> GetOrderByIdAsync(int orderId, string userId)
        {
            var order = await _context.Orders
                                            .Where(o => o.OrderId == orderId)
                                            .Include(o => o.OrderItems)
                                                .ThenInclude(oi => oi.Product)
                                                .Include(s => s.StatusNavigation)
                                            .Include(o => o.Payment)
                                                .ThenInclude(p => p.PaymentStatus)
                                            .Include(o => o.Payment)
                                                .ThenInclude(p => p.PaymentMethodNavigation)
                                            .Include(o => o.Shipping)
                                                .ThenInclude(s => s.ShippingStatus)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();

            if (order == null)
                return ServiceResult<Order?>.ErrorResult("Order not found", StatusCodes.Status404NotFound);

            if (order.UserId != userId)
                return ServiceResult<Order?>.ErrorResult("Unauthorized", StatusCodes.Status403Forbidden);

            return ServiceResult<Order?>.SuccessResult(order, message: "Order found successfully", statusCode: StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<List<Order>?>> GetOrdersByUserIdAsync(string userId)
        {
            var orders = await _context.Orders.Where(o => o.UserId == userId).ToListAsync();

            if (orders == null || orders.Count == 0)
                return ServiceResult<List<Order>?>.ErrorResult("Orders not found", StatusCodes.Status404NotFound);

            return ServiceResult<List<Order>?>.SuccessResult(orders, "Orders retrieved successfully", StatusCodes.Status200OK);
        }


        private bool IsValidStatusTransition(int currentStatus, int newStatus)
        {
            var validTransitions = new HashSet<int> { 1, 2, 3, 4, 5 };

            return validTransitions.Contains(currentStatus) && validTransitions.Contains(newStatus);
        }

    }
}
