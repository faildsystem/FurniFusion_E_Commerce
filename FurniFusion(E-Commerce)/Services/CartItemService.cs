using FurniFusion.Data;
using FurniFusion.Dtos.CartItem;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class CartItemService : ICartItemService
    {

        private readonly ICartService _cartService;
        private readonly FurniFusionDbContext _context;

        public CartItemService(ICartService cartService, FurniFusionDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        public async Task<ServiceResult<ShoppingCartItem?>> AddCartItemAsync(ShoppingCartItem shoppingCartItem, string userId)
        {

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == shoppingCartItem.ProductId);

            if (product == null)
                return ServiceResult<ShoppingCartItem?>.ErrorResult("Product not found", StatusCodes.Status404NotFound);

            var cart = await _cartService.GetCartAsync(userId);

            shoppingCartItem.CartId = cart.Data!.CartId;

            var existingCartItem = await _context.ShoppingCartItems.FirstOrDefaultAsync(s => s.ProductId == shoppingCartItem.ProductId && s.CartId == cart.Data!.CartId);

            if (existingCartItem != null)
                return ServiceResult<ShoppingCartItem?>.ErrorResult("Cart item already exists", StatusCodes.Status400BadRequest);

            var cartItem = await _context.ShoppingCartItems.AddAsync(shoppingCartItem);

            await _context.SaveChangesAsync();

            return ServiceResult<ShoppingCartItem?>.SuccessResult(cartItem.Entity, "Cart item added successfully", StatusCodes.Status201Created);

        }

        public async Task<ServiceResult<ShoppingCartItem?>> DeleteCartItemAsync(int productId, string userId)
        {
            var cart = await _cartService.GetCartAsync(userId);

            var existingCartItem = await _context.ShoppingCartItems.FirstOrDefaultAsync(s => s.ProductId == productId && s.CartId == cart.Data!.CartId);

            if (existingCartItem == null)
                return ServiceResult<ShoppingCartItem?>.ErrorResult("Cart item not found", StatusCodes.Status404NotFound);

            _context.ShoppingCartItems.Remove(existingCartItem!);
            await _context.SaveChangesAsync();

            return ServiceResult<ShoppingCartItem?>.SuccessResult(null, "Cart item deleted successfully", StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<ShoppingCartItem?>> UpdateCartItemQuanityAsync(UpdateCartItemQuantityDto quanityDto, int productId, string userId)
        {
            var cart = await _cartService.GetCartAsync(userId);

            var existingCartItem = await _context.ShoppingCartItems.FirstOrDefaultAsync(s => s.ProductId == productId && s.CartId == cart.Data!.CartId);

            if (existingCartItem == null)
                return ServiceResult<ShoppingCartItem?>.ErrorResult("Cart item not found", StatusCodes.Status404NotFound);

            existingCartItem.Quantity = quanityDto.Quantity;

            await _context.SaveChangesAsync();

            return ServiceResult<ShoppingCartItem?>.SuccessResult(existingCartItem, "Cart item quantity updated successfully", StatusCodes.Status200OK);
        }
    }
}
