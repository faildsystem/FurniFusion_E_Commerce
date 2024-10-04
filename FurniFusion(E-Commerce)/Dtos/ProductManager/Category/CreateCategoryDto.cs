using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.ProductManager.Category
{
    public class CreateCategoryDto
    {
        [Required]
        public string? CategoryName { get; set; }
    }
}