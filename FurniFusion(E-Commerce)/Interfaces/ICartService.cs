using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface ICartService
    {

        Task<ServiceResult<ShoppingCart?>> GetCartAsync(string userId);

        Task<ServiceResult<ShoppingCart?>> CreateCartAsync(ShoppingCart cart);

        Task<ServiceResult<ShoppingCart?>> DeleteCartAsync(string userId);

    }
}
