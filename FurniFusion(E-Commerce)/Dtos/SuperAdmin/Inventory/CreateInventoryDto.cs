using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin.Inventory
{
    public class CreateInventoryDto
    {
        [Required]
        public string WarehouseLocation { get; set; } = null!;

        [Required]
        public bool? IsActive { get; set; }

        [Required]
        public DateTime? CreatedAt { get; set; }

        [Required]
        public DateTime? UpdatedAt { get; set; }
    }
}