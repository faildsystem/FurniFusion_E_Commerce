using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.ProductManager.Discount
{
    public class CreateDiscountDto
    {
        [Required]
        public string DiscountCode { get; set; }

        [Required]
        public decimal DiscountValue { get; set; }

        [Required]
        public int DiscountUnitId { get; set; }
        
        [Required]
        public DateOnly ValidFrom { get; set; }

        [Required]
        public DateOnly ValidTo { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public decimal MaxAmount { get; set; }
    }
}
