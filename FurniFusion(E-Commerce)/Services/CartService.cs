using FurniFusion.Data;
using FurniFusion.Dtos.Cart;
using FurniFusion.Dtos.CartItem;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class CartService : ICartService
    {

        private readonly FurniFusionDbContext _context;

        public CartService(FurniFusionDbContext context)
        {
            _context = context;
        }


        public async Task<ShoppingCart> CreateCartAsync(ShoppingCart cart)
        {
            var createdCart = await _context.ShoppingCarts.AddAsync(cart);
            await _context.SaveChangesAsync();

            return createdCart.Entity;

        }

        public async Task<ServiceResult<ShoppingCartDTO?>> GetCartAsync(string userId)
        {
            var cartItems = await _context.ShoppingCarts
                .Where(c => c.UserId == userId)
                .Select(c => new ShoppingCartDTO
                {
                    CartId = c.CartId,
                    ShoppingCartItems = c.ShoppingCartItems.Select(i => new ShoppingCartItemDTO
                    {
                        ProductId = i.Product.ProductId,
                        ProductName = i.Product.ProductName,
                        ImageUrl = i.Product.ImageUrls!.FirstOrDefault(),
                        Price = i.Product.Price,
                        Quantity = i.Quantity ?? 0,
                        TotalPrice = i.Quantity * i.Product.Price ?? 0
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return ServiceResult<ShoppingCartDTO?>.SuccessResult(cartItems, message: "Cart retrieved successfully", statusCode: StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<ShoppingCartItem?>> AddCartItemAsync(ShoppingCartItem shoppingCartItem, string userId)
        {

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == shoppingCartItem.ProductId);

            if (product == null)
                return ServiceResult<ShoppingCartItem?>.ErrorResult("Product not found", StatusCodes.Status404NotFound);

            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == userId);

            shoppingCartItem.CartId = cart!.CartId;

            var existingCartItem = await _context.ShoppingCartItems.FirstOrDefaultAsync(s => s.ProductId == shoppingCartItem.ProductId && s.CartId == cart.CartId);

            if (existingCartItem != null)
                return ServiceResult<ShoppingCartItem?>.ErrorResult("Cart item already exists", StatusCodes.Status400BadRequest);

            var cartItem = await _context.ShoppingCartItems.AddAsync(shoppingCartItem);

            await _context.SaveChangesAsync();

            return ServiceResult<ShoppingCartItem?>.SuccessResult(cartItem.Entity, "Cart item added successfully", StatusCodes.Status201Created);

        }

        public async Task<ServiceResult<ShoppingCartItem?>> DeleteCartItemAsync(int productId, string userId)
        {
             var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == userId);

            var existingCartItem = await _context.ShoppingCartItems.FirstOrDefaultAsync(s => s.ProductId == productId && s.CartId == cart!.CartId);

            if (existingCartItem == null)
                return ServiceResult<ShoppingCartItem?>.ErrorResult("Cart item not found", StatusCodes.Status404NotFound);

            _context.ShoppingCartItems.Remove(existingCartItem!);
            await _context.SaveChangesAsync();

            return ServiceResult<ShoppingCartItem?>.SuccessResult(null, "Cart item deleted successfully", StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<bool>> RemoveAllCartItems(string userId)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart!.ShoppingCartItems.Count == 0)
                return ServiceResult<bool>.ErrorResult("Cart is already empty", StatusCodes.Status404NotFound);

            await _context.ShoppingCartItems.Where(s => s.CartId == cart.CartId).ExecuteDeleteAsync();

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true, "Cart items removed successfully", StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<ShoppingCartItem?>> UpdateCartItemQuanityAsync(UpdateCartItemQuantityDto quanityDto, int productId, string userId)
        {
            var cart = await GetCartAsync(userId);

            var existingCartItem = await _context.ShoppingCartItems.FirstOrDefaultAsync(s => s.ProductId == productId && s.CartId == cart.Data!.CartId);

            if (existingCartItem == null)
                return ServiceResult<ShoppingCartItem?>.ErrorResult("Cart item not found", StatusCodes.Status404NotFound);

            existingCartItem.Quantity = quanityDto.Quantity;

            await _context.SaveChangesAsync();

            return ServiceResult<ShoppingCartItem?>.SuccessResult(existingCartItem, "Cart item quantity updated successfully", StatusCodes.Status200OK);
        }

    }
}
