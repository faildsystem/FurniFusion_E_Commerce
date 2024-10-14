using FurniFusion.Dtos.ProductManager.DiscountUnit;
using FurniFusion.Dtos.ProductManager.Product;

namespace FurniFusion.Dtos.ProductManager.Discount
{
    public class DiscountDto
    {
        public int DiscountId { get; set; }

        public string? DiscountCode { get; set; }

        public decimal? DiscountValue { get; set; }

        public int? DiscountUnitId { get; set; }

        public DiscountUnitDto? DiscountUnit { get; set; }

        public DateOnly? ValidFrom { get; set; }

        public DateOnly? ValidTo { get; set; }

        public bool? IsActive { get; set; }

        public decimal? MaxAmount { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public ICollection<ProductDto>? Products { get; set; }

    }
}
