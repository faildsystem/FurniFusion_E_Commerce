using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface IWishlistItemService
    {
        Task<ServiceResult<WishlistItem?>> AddWhishlistItemAsync(int productId, string userId);

        Task<ServiceResult<WishlistItem?>> RemoveWhishlistItemAsync(int productId, string userId);

    }
}
