using FurniFusion.Dtos.ProductManager.Category;
using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface ICategoryService
    {
        Task<ServiceResult<List<Category>>> GetAllCategoriesAsync();
        Task<ServiceResult<Category>> GetCategoryByIdAsync(int? id);
        Task<ServiceResult<Category>> CreateCategoryAsync(CreateCategoryDto categoryDto, string creatorId);
        Task<ServiceResult<Category>> UpdateCategoryAsync(UpdateCategoryDto categoryDto, string updatorId);
        Task<ServiceResult<bool>> DeleteCategoryAsync(int id);
    }
}
