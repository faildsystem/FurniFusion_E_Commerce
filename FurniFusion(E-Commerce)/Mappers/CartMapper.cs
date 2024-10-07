using FurniFusion.Dtos.Cart;
using FurniFusion.Models;

namespace FurniFusion.Mappers
{
    public static class CartMapper
    {

        public static CartDto ToDto(this ShoppingCart cart)
        {
            return new CartDto
            {
                ShoppingCartItems = cart.ShoppingCartItems.Select(ci => ci.ToCartItemDto()).ToList()
            };
        }
    }
}
