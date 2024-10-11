using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin.Inventory
{
    public class UpdateInventoryDto
    {
        [Required]
        public int InventoryId { get; set; }

        public string WarehouseLocation { get; set; } = null!;

        public bool? IsActive { get; set; }

    }
}