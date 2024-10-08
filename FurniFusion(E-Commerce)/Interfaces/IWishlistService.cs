using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface IWishlistService
    {
        Task<Wishlist> CreateWishlistAsync(Wishlist wishlist);
        Task<ServiceResult<Wishlist?>> GetWishlistAsync(string userId);

        Task<ServiceResult<WishlistItem?>> AddWishlistItemAsync(int productId, string userId);

        Task<ServiceResult<WishlistItem?>> RemoveWishlistItemAsync(int productId, string userId);

        Task<ServiceResult<bool>> RemoveAllWishlistItemsAsync(string userId);

    }
}
