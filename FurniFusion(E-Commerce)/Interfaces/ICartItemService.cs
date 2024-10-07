using FurniFusion.Dtos.CartItem;
using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface ICartItemService
    {

        Task<ServiceResult<ShoppingCartItem?>> AddCartItemAsync(ShoppingCartItem shoppingCartItem, string userId);

        Task<ServiceResult<ShoppingCartItem?>> UpdateCartItemQuanityAsync(UpdateCartItemQuantityDto qurantityDto, int productId, string userId);

        Task<ServiceResult<ShoppingCartItem?>> DeleteCartItemAsync(int productId, string userId);

    }
}
