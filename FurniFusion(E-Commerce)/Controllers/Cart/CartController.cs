using FurniFusion.Interfaces;
using FurniFusion.Mappers;
using FurniFusion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers.Cart
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartService;


        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        [HttpGet]
        public async Task<IActionResult> GetCart()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ModelState);

            try
            {

                var result = await _cartService.GetCartAsync(userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data!.ShoppingCartItems.Select(ShoppingCartItem => ShoppingCartItem.ToCartItemDto()));
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ModelState);

            var cart = new ShoppingCart
            {
                UserId = userId,
                UpdatedAt = DateTime.Now,
            };

            try
            {
                var result = await _cartService.CreateCartAsync(cart);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return CreatedAtAction(nameof(CreateCart), new { id = result.Data!.CartId }, result.Data);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ModelState);

            try
            {
                var result = await _cartService.DeleteCartAsync(userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = "Cart deleted successfully" });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
