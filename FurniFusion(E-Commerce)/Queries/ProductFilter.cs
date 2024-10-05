namespace FurniFusion.Queries
{
    public class ProductFilter
    {
        public int? CategoryId { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public string? Name { get; set; }

        public decimal? Weight { get; set; }

        public string? Color { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public int? StockQuantity { get; set; }

        public bool? IsAvailable { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
