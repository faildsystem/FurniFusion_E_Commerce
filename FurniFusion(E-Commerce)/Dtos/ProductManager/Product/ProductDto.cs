using FurniFusion.Dtos.ProductManager.Discount;
using FurniFusion.Dtos.Review;
using FurniFusion.Dtos.SuperAdmin.InventoryProduct;

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

        public int? StockQuantity { get; set; } = 0;

        public bool? IsAvailable { get; set; } = false;

        public decimal? AverageRating { get; set; }


        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public int? DiscountId { get; set; }

        public int? CategoryId { get; set; }

        //public CategoryDto? Category { get; set; }
        public DiscountDto? Discount { get; set; }

        public ICollection<ReviewDto>? Reviews { get; set; }

        public List<InventoryProductDto> InventoryProducts { get; set; } = new List<InventoryProductDto>();

    }
}
