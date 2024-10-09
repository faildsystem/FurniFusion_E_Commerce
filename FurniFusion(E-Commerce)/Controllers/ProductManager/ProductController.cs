using FurniFusion.Dtos.ProductManager.Product;
using FurniFusion.Interfaces;
using FurniFusion.Mappers;
using FurniFusion.Queries;
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
                var Products = await _productService.GetAllProductsAsync(filter);

                var productsDto = Products.Select(p => p.ToProductDto()).ToList();

                var TotalProducts = Products.Count;

                return Ok(new { TotalProducts, productsDto });

            } catch (Exception e) {

                return BadRequest(new { message = e.Message });
            }
            
        }

        [HttpGet("getProduct/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = await _productService.GetProductByIdAsync(id);

                return Ok(product.ToProductDto());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto productDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var product = await _productService.CreateProductAsync(productDto, creatorId!);

                return Ok(product.ToProductDto());

            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

        }

        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var product = await _productService.UpdateProductAsync(productDto, updatorId!);

                return Ok(product.ToProductDto());

            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("deleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return Ok(new { message = "Product deleted" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
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
                var product = await _productService.ApplyDiscountToProduct(productId, discountId);
                return Ok(product.ToProductDto());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
