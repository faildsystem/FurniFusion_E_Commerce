using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.ProductManager.Discount
{
    public class CreateDiscountUnitDto
    {
        [Required]
        public string UnitName { get; set; }
    }
}
