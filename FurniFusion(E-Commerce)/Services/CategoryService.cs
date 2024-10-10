using FurniFusion.Data;
using FurniFusion.Dtos.ProductManager.Category;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly FurniFusionDbContext _context;

        public CategoryService(FurniFusionDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Category>>> GetAllCategoriesAsync()
        {

            var categories =  await _context.Categories.Include(p => p.Products).ToListAsync();

            return ServiceResult<List<Category>>.SuccessResult(categories);
        }

        public async Task<ServiceResult<Category>> GetCategoryByIdAsync(int? id)
        {
            var category = await _context.Categories.Include(p => p.Products).FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return ServiceResult<Category>.ErrorResult("Category not found", StatusCodes.Status404NotFound);
            }

            return ServiceResult<Category>.SuccessResult(category);
        }

        public async Task<ServiceResult<Category>> CreateCategoryAsync(CreateCategoryDto categoryDto, string creatorId)
        {
            var result = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryDto.CategoryName);

            if (result != null)
            {
                return ServiceResult<Category>.ErrorResult("Category already exists", StatusCodes.Status409Conflict);
            }

            var newCategory = new Category
            {
                CategoryName = categoryDto.CategoryName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatedBy = creatorId,
                UpdatedBy = creatorId
            };

            await _context.Categories.AddAsync(newCategory);
            await _context.SaveChangesAsync();

            return ServiceResult<Category>.SuccessResult(newCategory, "Category created successfully", StatusCodes.Status201Created);
        }

        public async Task<ServiceResult<Category>> UpdateCategoryAsync(UpdateCategoryDto categoryDto, string updatorId)
        {
            var result = await GetCategoryByIdAsync(categoryDto.CategoryId);

            if (result == null || result.Data == null)
            {
                return ServiceResult<Category>.ErrorResult("Category not found", StatusCodes.Status404NotFound);
            }

            result.Data.CategoryName = categoryDto.NewCategoryName;
            result.Data.UpdatedAt = DateTime.Now;
            result.Data.UpdatedBy = updatorId;

            _context.Categories.Update(result.Data);
            await _context.SaveChangesAsync();
            
            return ServiceResult<Category>.SuccessResult(result.Data, "Category updated successfully");
        }

        public async Task<ServiceResult<bool>> DeleteCategoryAsync(int id)
        {
            var result = await GetCategoryByIdAsync(id);

            if (result == null || result.Data == null)
            {
                return ServiceResult<bool>.ErrorResult("Category not found", StatusCodes.Status404NotFound);
            }

            _context.Categories.Remove(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true, "Category deleted successfully");
        }
    }
}
