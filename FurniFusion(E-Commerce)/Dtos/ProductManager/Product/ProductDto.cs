using FurniFusion.Dtos.ProductManager.Category;
using FurniFusion.Dtos.ProductManager.Discount;

namespace FurniFusion.Dtos.ProductManager.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public List<string>? ImageUrls { get; set; }

        public string? Dimensions { get; set; }

        public decimal? Weight { get; set; }

        public List<string>? Colors { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int? StockQuantity { get; set; }

        public bool? IsAvailable { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public int? DiscountId { get; set; }

        public DiscountDto? Discount { get; set; }

        public int? CategoryId { get; set; }

        public CategoryDto? Category { get; set; }

        public decimal? AverageRating { get; set; }
    }
}
