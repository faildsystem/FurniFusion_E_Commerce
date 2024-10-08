using FurniFusion.Data;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly FurniFusionDbContext _context;

        public WishlistService(FurniFusionDbContext context)
        {
            _context = context;
        }

        public async Task<Wishlist> CreateWishlistAsync(Wishlist wishlist)
        {
            var createdWishlist = await _context.Wishlists.AddAsync(wishlist);

            await _context.SaveChangesAsync();

            return createdWishlist.Entity;

        }

        public async Task<ServiceResult<Wishlist?>> GetWishlistAsync(string userId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.WishlistItems)
                .ThenInclude(w => w.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            return ServiceResult<Wishlist?>.SuccessResult(wishlist, "Wishlist retrieved successfully", StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<WishlistItem?>> AddWishlistItemAsync(int productId, string userId)
        {
            var userWishlist = await GetWishlistAsync(userId);

            var existingWishlistItem = userWishlist.Data!.WishlistItems.FirstOrDefault(w => w.ProductId == productId);

            if (existingWishlistItem != null)
                return ServiceResult<WishlistItem?>.ErrorResult("Wishlist item already exists", StatusCodes.Status400BadRequest);

            var wishlistItem = new WishlistItem
            {
                WishlistId = userWishlist.Data!.WishlistId,
                ProductId = productId,
                CreatedAt = DateTime.Now
            };

            var createdWishlistItem = await _context.WishlistItems.AddAsync(wishlistItem);

            await _context.SaveChangesAsync();

            return ServiceResult<WishlistItem?>.SuccessResult(createdWishlistItem.Entity, "Wishlist item added successfully", StatusCodes.Status201Created);


        }

        public async Task<ServiceResult<bool>> RemoveAllWishlistItemsAsync(string userId)
        {
            var userWishlist = await GetWishlistAsync(userId);

            if (userWishlist.Data!.WishlistItems.Count == 0)
                return ServiceResult<bool>.ErrorResult("Wishlist is already empty", StatusCodes.Status404NotFound);

            _context.WishlistItems.RemoveRange(userWishlist.Data!.WishlistItems);

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true, "Wishlist items removed successfully", StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<WishlistItem?>> RemoveWishlistItemAsync(int productId, string userId)
        {
            var userWishlist = await GetWishlistAsync(userId);

            var existingWishlistItem = userWishlist.Data!.WishlistItems.FirstOrDefault(w => w.ProductId == productId);

            if (existingWishlistItem == null)
                return ServiceResult<WishlistItem?>.ErrorResult("Wishlist item not found", StatusCodes.Status404NotFound);

            _context.WishlistItems.Remove(existingWishlistItem);

            await _context.SaveChangesAsync();

            return ServiceResult<WishlistItem?>.SuccessResult(null, "Wishlist item removed successfully", StatusCodes.Status200OK);

        }
    }
}
