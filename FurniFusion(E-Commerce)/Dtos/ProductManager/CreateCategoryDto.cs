using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.ProductManager
{
    public class CreateCategoryDto
    {
        [Required]
        public string? CategoryName { get; set; }
    }
}