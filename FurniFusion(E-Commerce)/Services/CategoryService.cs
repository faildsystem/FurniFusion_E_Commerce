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

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories =  await _context.Categories.ToListAsync();

            return categories;
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryDto categoryDto, string creatorId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryDto.CategoryName);

            if (category != null)
            {
                throw new Exception("Category already exists");
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
            return newCategory;
        }

        public async Task<Category> UpdateCategoryAsync(UpdateCategoryDto categoryDto, string updatorId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryDto.CategoryId);

            if (category == null)
            {
                throw new Exception("Category not found");
            }

            category.CategoryName = categoryDto.NewCategoryName;
            category.UpdatedAt = DateTime.Now;
            category.UpdatedBy = updatorId;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                throw new Exception("Category not found");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
