using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.ProductManager.Discount
{
    public class UpdateDiscountUnitDto
    {
        [Required]
        public int? UnitId { get; set; }

        [Required]
        public string UnitName { get; set; }
    }
}
