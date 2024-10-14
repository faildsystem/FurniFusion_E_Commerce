using FurniFusion.Dtos.ProductManager.Product;
using FurniFusion.Interfaces;
using FurniFusion.Mappers;
using FurniFusion.Queries;
using FurniFusion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers
{
    [Authorize(Roles = "superAdmin, productManager")]
    [ApiController]
    [Route(("api/[controller]"))]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;




        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getProducts")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try {

                var result = await _productService.GetAllProductsAsync(filter);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                var products = result.Data.Select(p => p.ToProductDto()).ToList();

                var TotalProducts = result.Data.Count;

                Console.WriteLine(TotalProducts);

                return Ok(new { TotalProducts, products });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpGet("getProduct/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //try
            //{
                var result = await _productService.GetProductByIdAsync(id);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data.ToProductDto());
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, new { message = ex.Message });
            //}
        }

        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto productDto)
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

                var result = await _productService.CreateProductAsync(productDto, creatorId!);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return CreatedAtAction(nameof(GetProduct), new { id = result.Data.ProductId }, result.Data.ToProductDto());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto productDto)
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

                var result = await _productService.UpdateProductAsync(productDto, updatorId!);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data.ToProductDto());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("deleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               var result = await _productService.DeleteProductAsync(id);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpPost("{productId}/applyDiscount/{discountId}")]
        public async Task<IActionResult> ApplyDiscountToProduct(int productId, int discountId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _productService.ApplyDiscountToProductAsync(productId, discountId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = "Discount applied successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
