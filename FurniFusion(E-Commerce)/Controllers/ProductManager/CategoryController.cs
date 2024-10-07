using FurniFusion.Dtos.ProductManager.Category;
using FurniFusion.Interfaces;
using FurniFusion.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers
{
    [Authorize(Roles = "superAdmin, productManager")]
    [ApiController]
    [Route(("api/[controller]"))]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _CategoryService;

        public CategoryController(ICategoryService CategoryService)
        {
            _CategoryService = CategoryService;
        }

        [HttpGet("getCategories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _CategoryService.GetAllCategoriesAsync();

                var categoriesDto = categories.Select(c => c.ToCategoryDto()).ToList();

                var totalItems = categories.Count();

                return Ok(new { totalItems, categoriesDto });

            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("createCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var category = await _CategoryService.CreateCategoryAsync(categoryDto, creatorId!);

                return Ok(category.ToCategoryDto());

            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut("updateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var category = await _CategoryService.UpdateCategoryAsync(categoryDto, updatorId!);

                return Ok(category.ToCategoryDto());

            }
            catch (Exception e)
            {
                return BadRequest( new { message = e.Message });
            }
        }

        [HttpDelete("deleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _CategoryService.DeleteCategoryAsync(id);
                return Ok(new { message = "Category deleted" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
