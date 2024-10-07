using FurniFusion.Data;
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


        public async Task<ServiceResult<ShoppingCart?>> CreateCartAsync(ShoppingCart cart)
        {
            var existingCart = await _context.ShoppingCarts.FirstOrDefaultAsync(cart => cart.UserId == cart.UserId);

            if (existingCart != null)
                return ServiceResult<ShoppingCart?>.ErrorResult("Cart already exists", StatusCodes.Status400BadRequest);

            var createdCart = await _context.ShoppingCarts.AddAsync(cart);
            await _context.SaveChangesAsync();

            return ServiceResult<ShoppingCart?>.SuccessResult(createdCart.Entity, "Cart created successfully", StatusCodes.Status201Created);

        }

        public async Task<ServiceResult<ShoppingCart?>> DeleteCartAsync(string userId)
        {
            var cart = await _context.ShoppingCarts.FirstOrDefaultAsync(cart => cart.UserId == userId);

            if (cart == null)
                return ServiceResult<ShoppingCart?>.ErrorResult("Cart not found", StatusCodes.Status404NotFound);

            _context.ShoppingCarts.Remove(cart);
            await _context.SaveChangesAsync();

            return ServiceResult<ShoppingCart?>.SuccessResult(cart, "Cart deleted successfully", StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<ShoppingCart?>> GetCartAsync(string userId)
        {
            // Fetch the cart and include associated cart items
            var cart = await _context.ShoppingCarts
                                     .Include(c => c.ShoppingCartItems)  // Ensure cart items are loaded
                                     .ThenInclude(c => c.Product)
                                     .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return await CreateCartAsync(new ShoppingCart
                {
                    UserId = userId,
                    UpdatedAt = DateTime.Now,
                    //ShoppingCartItems = new List<ShoppingCartItem>()  // Initialize empty cart items
                });

            return ServiceResult<ShoppingCart?>.SuccessResult(cart);
        }

    }
}
