using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.ProductManager.Discount
{
    public class UpdateDiscountDto
    {
        [Required]
        public int? DiscountId { get; set; }

        public string? DiscountCode { get; set; }

        public decimal? DiscountValue { get; set; }

        public int? DiscountUnitId { get; set; }

        public DateOnly? ValidFrom { get; set; }

        public DateOnly? ValidTo { get; set; }

        public bool? IsActive { get; set; }

        public decimal? MaxAmount { get; set; }
    }
}
