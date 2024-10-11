using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin.InventoryProduct
{
    public class CreateInventoryProductDto
    {
        [Required]
        public int InventoryId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int? Quantity { get; set; }

    }
}