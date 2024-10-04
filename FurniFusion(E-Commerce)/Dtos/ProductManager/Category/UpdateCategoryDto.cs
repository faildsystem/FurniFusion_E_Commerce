using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.ProductManager.Category
{
    public class UpdateCategoryDto
    {
        [Required]
        public int? CategoryId { get; set; }

        [Required]
        public string? NewCategoryName { get; set; }
    }
}
