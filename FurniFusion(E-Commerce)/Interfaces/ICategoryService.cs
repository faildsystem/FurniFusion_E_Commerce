using FurniFusion.Dtos.ProductManager.Category;
using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> CreateCategoryAsync(CreateCategoryDto categoryDto, string creatorId);
        Task<Category> UpdateCategoryAsync(UpdateCategoryDto categoryDto, string updatorId);
        Task DeleteCategoryAsync(int id);
    }
}
