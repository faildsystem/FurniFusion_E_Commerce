using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin.InventoryProduct
{
    public class UpdateInventoryProductDto
    {
        [Required]
        public int InventoryId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int? Quantity { get; set; }

    }
}