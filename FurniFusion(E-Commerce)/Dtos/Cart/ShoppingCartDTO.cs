using FurniFusion.Dtos.CartItem;

namespace FurniFusion.Dtos.Cart
{
    public class ShoppingCartDTO
    {
        public int CartId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ShoppingCartItemDTO> ShoppingCartItems { get; set; } = new();
    }
}
