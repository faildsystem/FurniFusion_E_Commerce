using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.ProductManager
{
    public class UpdateCategoryDto 
    {
        [Required]
        public int? CategoryId { get; set; }

        [Required]
        public string? NewCategoryName { get; set; }
    }
}
