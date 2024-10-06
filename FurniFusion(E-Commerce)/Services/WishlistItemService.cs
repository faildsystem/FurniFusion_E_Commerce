using FurniFusion.Data;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class WishlistItemService : IWishlistItemService
    {

        private readonly FurniFusionDbContext _context;
        private readonly IWishlistService _wishlistService;

        public WishlistItemService(FurniFusionDbContext context, IWishlistService wishlistService)
        {
            _context = context;
            _wishlistService = wishlistService;
        }


        public async Task<ServiceResult<WishlistItem?>> AddWhishlistItemAsync(int productId, string userId)
        {
            var userWishlist = await _wishlistService.GetWishlistAsync(userId);

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

        public async Task<ServiceResult<WishlistItem?>> RemoveWhishlistItemAsync(int productId, string userId)
        {
            var userWishlist = await _wishlistService.GetWishlistAsync(userId);

            var existingWishlistItem = userWishlist.Data!.WishlistItems.FirstOrDefault(w => w.ProductId == productId);

            if (existingWishlistItem == null)
                return ServiceResult<WishlistItem?>.ErrorResult("Wishlist item not found", StatusCodes.Status404NotFound);

            _context.WishlistItems.Remove(existingWishlistItem);

            await _context.SaveChangesAsync();

            return ServiceResult<WishlistItem?>.SuccessResult(null, "Wishlist item removed successfully", StatusCodes.Status200OK);

        }
    }
}
