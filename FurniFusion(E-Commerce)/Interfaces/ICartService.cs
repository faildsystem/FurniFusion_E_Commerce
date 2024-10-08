using FurniFusion.Dtos.CartItem;
using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface ICartService
    {

        Task<ServiceResult<ShoppingCart?>> GetCartAsync(string userId);

        Task<ShoppingCart> CreateCartAsync(ShoppingCart cart);

        Task<ServiceResult<ShoppingCartItem?>> AddCartItemAsync(ShoppingCartItem shoppingCartItem, string userId);

        Task<ServiceResult<ShoppingCartItem?>> UpdateCartItemQuanityAsync(UpdateCartItemQuantityDto qurantityDto, int productId, string userId);

        Task<ServiceResult<ShoppingCartItem?>> DeleteCartItemAsync(int productId, string userId);

        Task<ServiceResult<bool>> RemoveAllCartItems(string userId);


    }
}
