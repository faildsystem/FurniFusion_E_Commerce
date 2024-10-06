using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.CartItem
{
    public class UpdateCartItemQuantityDto
    {
        [Required]
        public int? Quantity { get; set; }
    }
}
