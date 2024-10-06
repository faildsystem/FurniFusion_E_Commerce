using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.CartItem
{
    public class AddCartItemDto
    {
        [Required]
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
