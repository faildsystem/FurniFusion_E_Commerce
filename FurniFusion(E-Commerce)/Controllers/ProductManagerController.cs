//using FurniFusion.Dtos.ProductManager;
//using FurniFusion.Interfaces;
//using FurniFusion.Models;
//using FurniFusion.Queries;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace FurniFusion.Controllers
//{
//    [Authorize(Roles = "superAdmin, productManager")]
//    [ApiController]
//    [Route(("api/[controller]"))]
//    public class ProductManagerController : ControllerBase
//    {
//        private readonly IProductManagerRepository _productManagerRepository;
//        private readonly UserManager<User> _userManager;
//        private readonly ITokenService _tokenService;

//        public ProductManagerController(UserManager<User> userManager, ITokenService tokenService, IProductManagerRepository productManagerRepository)
//        {
//            _userManager = userManager;
//            _tokenService = tokenService;
//            _productManagerRepository = productManagerRepository;
//        }

//        [HttpGet("getProducts")]
//        public async Task<IActionResult> GetProducts([FromQuery] ProductFilter filter)
//        {
//            var Products = await _productManagerRepository.GetAllProductsAsync(filter);
//            var TotalProducts = Products.Count;



//            return Ok(new {TotalProducts, Products});
//        }

//        [HttpPost("createProduct")]
//        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
//        {

//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//                var product = await _productManagerRepository.CreateProductAsync(productDto, creatorId);

//                return Ok(product);

//            }
//            catch (Exception e)
//            {
//                return BadRequest(e.Message);
//            }

//        }

//        [HttpPut("updateProduct")]
//        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto productDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var product = await _productManagerRepository.UpdateProductAsync(productDto);

//                return Ok(product);

//            }
//            catch (Exception e)
//            {
//                return BadRequest(e.Message);
//            }
//        }

//        [HttpDelete("deleteProduct/{id}")]
//        public async Task<IActionResult> DeleteProduct(int id)
//        {
//            try
//            {
//                await _productManagerRepository.DeleteProductAsync(id);
//                return Ok(new {message = "Product deleted"});
//            }
//            catch (Exception e)
//            {
//                return BadRequest(e.Message);
//            }
//        }

//        [HttpGet("getCategories")]
//        public async Task<IActionResult> GetCategories()
//        {
//            var categories = await _productManagerRepository.GetAllCategoriesAsync();
//            return Ok(categories);
//        }
//        [HttpPost("createCategory")]
//        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto categoryDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//                var category = await _productManagerRepository.CreateCategoryAsync(categoryDto, creatorId!);

//                return Ok(category);

//            }
//            catch (Exception e)
//            {
//                return BadRequest(e.Message);
//            }
//        }

//        [HttpPut("updateCategory")]
//        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDto categoryDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var updatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//                var category = await _productManagerRepository.UpdateCategoryAsync(categoryDto, updatorId!);

//                return Ok(category);

//            }
//            catch (Exception e)
//            {
//                return BadRequest(e.Message);
//            }
//        }

//        [HttpDelete("deleteCategory/{id}")]
//        public async Task<IActionResult> DeleteCategory(int id)
//        {
//            try
//            {
//                await _productManagerRepository.DeleteCategoryAsync(id);
//                return Ok(new {message = "Category deleted"});
//            }
//            catch (Exception e)
//            {
//                return BadRequest(e.Message);
//            }
//        }

//    }
//}
