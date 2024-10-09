using FurniFusion.Dtos.CartItem;
using FurniFusion.Interfaces;
using FurniFusion.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers.Cart
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
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
                var result = await _cartItemService.AddCartItemAsync(cartItem, userId);

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
                var result = await _cartItemService.UpdateCartItemQuanityAsync(quantityDto, productId, userId);

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
                var result = await _cartItemService.DeleteCartItemAsync(productId, userId);

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
