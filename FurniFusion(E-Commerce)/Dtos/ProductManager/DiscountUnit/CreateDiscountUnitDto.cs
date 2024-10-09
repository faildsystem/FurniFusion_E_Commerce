using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.ProductManager.DiscountUnit
{
    public class CreateDiscountUnitDto
    {
        [Required]
        public string UnitName { get; set; }
    }
}
