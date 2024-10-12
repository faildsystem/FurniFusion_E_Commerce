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

        public async Task<ServiceResult<bool>> CancelOrderAsync(int orderId, string userId)
        {
            var order = await _context.Orders
                .Where(o => o.OrderId == orderId)
                .FirstOrDefaultAsync();

            //var order = from o in _context.Orders
            //            join p in _context.Payments
            //            on o.PaymentId equals p.PaymentId
            //            join d in _context.Discounts
            //            on o.DiscountId equals d.DiscountId
            //            join i in _context.OrderItems
            //            on o.OrderId equals i.OrderId
            //            where o.OrderId == orderId
            //            select new
            //            {
            //               o.OrderId,
            //               o.UserId,
            //               o.Status,
            //               o.UpdatedAt,
            //               o.TotalPrice,



            //            };

            if (order == null)
                return ServiceResult<bool>.ErrorResult("Order not found", StatusCodes.Status404NotFound);

            if (order.UserId != userId)
                return ServiceResult<bool>.ErrorResult("Unauthorized", StatusCodes.Status403Forbidden);

            if (order.Status == 4)
                return ServiceResult<bool>.ErrorResult("Order already canceled", StatusCodes.Status400BadRequest);

            if (order.Status != 1)
                return ServiceResult<bool>.ErrorResult("Order cannot be canceled", StatusCodes.Status400BadRequest);


            var payment = await _paymentService.GetPaymentByIdAsync((int)order.PaymentId!);

            if (payment == null)
                return ServiceResult<bool>.ErrorResult("Payment not found", StatusCodes.Status404NotFound);

            order.Status = 4;
            order.UpdatedAt = DateTime.UtcNow;
            payment.PaymentStatusId = 4;

            await _context.SaveChangesAsync();


            return ServiceResult<bool>.SuccessResult(true, "Order canceled successfully", StatusCodes.Status200OK);

        }

        public async Task<ServiceResult<UpdateOrdersStatusDto?>> ChangeOrderStatusAsync(Dictionary<int, int> data)
        {
            var updateOrdersStatusDto = new UpdateOrdersStatusDto();

            // Fetch orders that match the provided OrderIds in a single query
            var orders = await _context.Orders
                                       .Where(o => data.Keys.Contains(o.OrderId))
                                       .ToListAsync();

            if (!orders.Any())
            {
                return ServiceResult<UpdateOrdersStatusDto?>.ErrorResult("Orders not found", StatusCodes.Status404NotFound);
            }

            // Track failed updates separately from the start
            var failedOrders = new HashSet<int>(data.Keys.Except(orders.Select(o => o.OrderId)));

            foreach (var order in orders)
            {
                if (data.TryGetValue(order.OrderId, out var newStatus))
                {
                    order.Status = newStatus; // Update the status
                    updateOrdersStatusDto.UpdatedOrders.Add(order.OrderId); // Track updated orders
                }
            }

            // Combine failed orders (those that didn't match any OrderId in the database)
            updateOrdersStatusDto.FailedOrders.AddRange(failedOrders);

            // Save changes only if there are updated orders
            if (updateOrdersStatusDto.UpdatedOrders.Any())
            {
                await _context.SaveChangesAsync();
            }

            return ServiceResult<UpdateOrdersStatusDto?>.SuccessResult(updateOrdersStatusDto, "Orders updated successfully", StatusCodes.Status200OK);
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

            var cartProducts = from product in _context.Products
                               join discount in _context.Discounts
                               on product.DiscountId equals discount.DiscountId into discountGroup
                               from discount in discountGroup.DefaultIfEmpty() // Left Join
                               join discountUnit in _context.DiscountUnits
                               on discount.DiscountUnitId equals discountUnit.UnitId into discountUnitGroup
                               from discountUnit in discountUnitGroup.DefaultIfEmpty() // Left Join
                               where productsIds.Contains(product.ProductId)
                               select new
                               {
                                   product.ProductId,
                                   product.ProductName,
                                   DiscountValue = discount != null ? discount.DiscountValue : 0, // Handle null in case of no match
                                   UnitName = discountUnit != null ? discountUnit.UnitName : null, // Handle null in case of no match
                                   product.Price,
                                   product.StockQuantity
                               };

            var orderItems = new List<OrderItem>();

            foreach (var product in cartProducts)
            {
                var requiredQuantity = cartItems.FirstOrDefault(c => c.ProductId == product.ProductId)!.Quantity;

                if (requiredQuantity == null || requiredQuantity < 1)
                    return ServiceResult<bool>.ErrorResult($" {product.ProductName} has Invalid quantity", StatusCodes.Status400BadRequest);

                if (product.StockQuantity < requiredQuantity)
                    return ServiceResult<bool>.ErrorResult($" {product.ProductName} Not enough stock", StatusCodes.Status400BadRequest);

                decimal priceAfterDiscount = 0;

                if (product.UnitName == "PERCENT")
                    priceAfterDiscount = (decimal)(product.Price * (1 - product.DiscountValue / 100));
                else
                    priceAfterDiscount = (decimal)(product.Price - product.DiscountValue);

                await _context.Products.Where(p => p.ProductId == product.ProductId).ExecuteUpdateAsync(p => p.SetProperty(p => p.StockQuantity, p => p.StockQuantity - requiredQuantity));


                orderItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    Quantity = (int)requiredQuantity,
                    Price = priceAfterDiscount,
                    CreatedAt = DateTime.Now,
                });
            }

            var totalPrice = orderItems.Sum(i => i.Price * i.Quantity);

            var payment = new Payment
            {
                Amount = totalPrice,
                UserId = userId,
                Date = DateTime.Now.Date,
                PaymentStatusId = 1,
                PaymentMethod = 1,
                TransactionId = Guid.NewGuid().ToString(),
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
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

                scope.Complete();
            }


            return ServiceResult<bool>.SuccessResult(true, "Order created successfully", StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<Order?>> GetOrderByIdAsync(int orderId, string userId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(c => c.OrderId == orderId);

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

    }
}
