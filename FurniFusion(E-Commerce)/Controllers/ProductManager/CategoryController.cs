using FurniFusion.Dtos.ProductManager.Category;
using FurniFusion.Interfaces;
using FurniFusion.Mappers;
using FurniFusion.Models;
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
                var result = await _CategoryService.GetAllCategoriesAsync();

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });


                var categories = result.Data.Select(c => c.ToCategoryDto()).ToList();

                var totalItems = result.Data.Count();

                return Ok(new { totalItems, categories });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("getCategory/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _CategoryService.GetCategoryByIdAsync(id);

                if (!result.Success)
                {
                    return StatusCode(result.StatusCode, new { message = result.Message });
                }

                return Ok(result.Data.ToCategoryDto());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("createCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(creatorId))
                return Unauthorized(new { message = "User not found" });


            try
            {
                var result = await _CategoryService.CreateCategoryAsync(categoryDto, creatorId!);

                if (!result.Success)
                    {
                    return StatusCode(result.StatusCode, new { message = result.Message });
                }

                return CreatedAtAction(nameof(CreateCategory), new { id = result.Data!.CategoryId }, result.Data.ToCategoryDto());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("updateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(updatorId))
                return Unauthorized(new { message = "User not found" });

            try
            {
                var result = await _CategoryService.UpdateCategoryAsync(categoryDto, updatorId!);

                if (!result.Success)
                {
                    return StatusCode(result.StatusCode, new { message = result.Message });
                }

                return Ok(result.Data.ToCategoryDto());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("deleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _CategoryService.DeleteCategoryAsync(id);

                if (!result.Success)
                {
                    return StatusCode(result.StatusCode, new { message = result.Message });
                }

                return Ok(new { message = "Category deleted successfully" });
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
