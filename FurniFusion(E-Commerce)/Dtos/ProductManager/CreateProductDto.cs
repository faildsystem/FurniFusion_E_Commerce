using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.ProductManager
{
    public class CreateProductDto
    {
        [Required]
        public string? ProductName { get; set; }

        [Required]
        public List<string>? ImageUrls { get; set; }

        [Required]
        public string? Dimensions { get; set; }

        [Required]
        public decimal? Weight { get; set; }

        [Required]
        public string? Color { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public int? StockQuantity { get; set; }

        [Required]
        public bool? IsAvailable { get; set; }

        [Required]
        public int? CategoryId { get; set; }
    }
}
