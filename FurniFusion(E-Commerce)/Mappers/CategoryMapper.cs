using FurniFusion.Models;
using FurniFusion.Dtos.ProductManager.Category;

namespace FurniFusion.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CreatedBy = category.CreatedBy,
                CreatedAt = category.CreatedAt,
                UpdatedBy = category.UpdatedBy,
                UpdatedAt = category.UpdatedAt,
                Products = category.Products.Select(p => p.ToProductDto()).ToList()
            };
        }
    }
}
