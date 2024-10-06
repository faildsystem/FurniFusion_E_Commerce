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

        public async Task<ServiceResult<Wishlist?>> CreateWishlistAsync(Wishlist wishlist)
        {
            var wishlistExists = await _context.Wishlists.FirstOrDefaultAsync(w => w.UserId == wishlist.UserId);

            if (wishlistExists != null)
                return ServiceResult<Wishlist?>.ErrorResult("Wishlist already exists", StatusCodes.Status400BadRequest);

            var createdWishlist = await _context.Wishlists.AddAsync(wishlist);

            await _context.SaveChangesAsync();

            return ServiceResult<Wishlist?>.SuccessResult(createdWishlist.Entity, "Wishlist created successfully", StatusCodes.Status201Created);
        }

        public async Task<ServiceResult<Wishlist?>> DeleteWishlistAsync(string userId)
        {
            var wishlist = await _context.Wishlists.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
                return ServiceResult<Wishlist?>.ErrorResult("Wishlist not found", StatusCodes.Status404NotFound);

            _context.Wishlists.Remove(wishlist);

            await _context.SaveChangesAsync();

            return ServiceResult<Wishlist?>.SuccessResult(null, "Wishlist deleted successfully", StatusCodes.Status200OK);

        }

        public async Task<ServiceResult<Wishlist?>> GetWishlistAsync(string userId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.WishlistItems)
                .ThenInclude(w => w.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
                return await CreateWishlistAsync(new Wishlist { UserId = userId, UpdatedAt = DateTime.Now });

            return ServiceResult<Wishlist?>.SuccessResult(wishlist, "Wishlist retrieved successfully", StatusCodes.Status200OK);
        }
    }
}
