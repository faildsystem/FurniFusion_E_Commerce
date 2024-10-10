using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers.user
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class WishlistController : ControllerBase
    {

        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService whishlistService)
        {
            _wishlistService = whishlistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });


            try
            {

                var result = await _wishlistService.GetWishlistAsync(userId);


                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data!.WishlistItems);
            }

            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }


        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddWishlistItem([FromRoute] int productId)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });


            try
            {

                var result = await _wishlistService.AddWishlistItemAsync(productId, userId);
                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });


                return StatusCode(result.StatusCode, new { message = result.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteWishlistItem([FromRoute] int productId)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });


            try
            {

                var result = await _wishlistService.RemoveWishlistItemAsync(productId, userId);


                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return StatusCode(result.StatusCode, new { message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });


            }
        }


        [HttpDelete]

        public async Task<IActionResult> DeleteAllWishlistItems()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });


            try
            {

                var result = await _wishlistService.RemoveAllWishlistItemsAsync(userId);


                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return StatusCode(result.StatusCode, new { message = result.Message });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
