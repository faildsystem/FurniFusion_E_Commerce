using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin.InventoryProduct
{
    public class DeleteInventoryProductDto
    {
        [Required]
        public int InventoryId { get; set; }

        [Required]
        public int ProductId { get; set; }

    }
}