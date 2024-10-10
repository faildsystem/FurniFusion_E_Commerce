using FurniFusion.Dtos.CartItem;
using FurniFusion.Interfaces;
using FurniFusion.Mappers;
using FurniFusion.Models;
using FurniFusion.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers.user
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
        public async Task<IActionResult> AddCartItem([FromBody] AddCartItemDto addCartItemDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            var cartItem = addCartItemDto.ToShoppingCartItem();

            try
            {
                var result = await _cartService.AddCartItemAsync(cartItem, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return CreatedAtAction(nameof(AddCartItem), new { message = result.Message }, result.Data.ToCartItemDto());


            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateCartItem([FromRoute] int productId, [FromBody] UpdateCartItemQuantityDto quantityDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ModelState);

            try
            {
                var result = await _cartService.UpdateCartItemQuanityAsync(quantityDto, productId, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteCartItem([FromRoute] int productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            try
            {
                var result = await _cartService.DeleteCartItemAsync(productId, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }


        [HttpDelete]
        public async Task<IActionResult> RemoveAllCartItem()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            try
            {
                var result = await _cartService.RemoveAllCartItems(userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }


        }

    }
}
