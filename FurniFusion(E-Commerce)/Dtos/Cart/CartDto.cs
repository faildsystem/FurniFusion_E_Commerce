using FurniFusion.Dtos.CartItem;

namespace FurniFusion.Dtos.Cart
{
    public class CartDto
    {
        public DateTime UpdatedAt { get; set; }
        public List<CartItemDto> ShoppingCartItems { get; set; } = new();
    }
}
