using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Queries
{
    public class DiscountFilter
    {
        public int? DiscountId { get; set; }

        public string? DiscountCode { get; set; }

        public decimal? DiscountValue { get; set; }

        public int? DiscountUnitId { get; set; }

        public DateOnly? ValidFrom { get; set; }

        public DateOnly? ValidTo { get; set; }

        public bool? IsActive { get; set; }

        public decimal? MaxAmount { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
