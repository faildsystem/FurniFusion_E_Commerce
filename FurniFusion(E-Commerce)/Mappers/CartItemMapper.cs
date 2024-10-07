using FurniFusion.Dtos.CartItem;
using FurniFusion.Models;

namespace FurniFusion.Mappers
{
    public static class CartItemMapper
    {

        public static CartItemDto ToCartItemDto(this ShoppingCartItem cartItem)
        {
            return new CartItemDto
            {
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.ProductName,
                ImageUrl = cartItem.Product.ImageUrls.FirstOrDefault(),  // Take the first image URL
                Price = cartItem.Product.Price,
                Quantity = (int) cartItem.Quantity
            };
        }

        public static ShoppingCartItem ToShoppingCartItem(this AddCartItemDto cartItemDto)
        {
            return new ShoppingCartItem
            {
                ProductId = (int) cartItemDto.ProductId,
                Quantity = cartItemDto.Quantity,
                CreatedAt = DateTime.Now,
            };
        }

    }
}
