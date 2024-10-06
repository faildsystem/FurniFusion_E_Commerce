using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface IWishlistService
    {
        Task<ServiceResult<Wishlist?>> GetWishlistAsync(string userId);
        Task<ServiceResult<Wishlist?>> CreateWishlistAsync(Wishlist wishlist);

        Task<ServiceResult<Wishlist?>> DeleteWishlistAsync(string userId);
    }
}
