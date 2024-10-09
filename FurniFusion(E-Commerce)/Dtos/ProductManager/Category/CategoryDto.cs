using FurniFusion.Dtos.ProductManager.Product;

namespace FurniFusion.Dtos.ProductManager.Category
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<ProductDto> Products { get; set; }

    }
}
