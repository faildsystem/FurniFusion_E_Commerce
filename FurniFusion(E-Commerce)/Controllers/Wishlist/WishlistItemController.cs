using FurniFusion.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers.Wishlist
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class WishlistItemController : ControllerBase
    {

        private readonly IWishlistItemService _wishlistItemService;

        public WishlistItemController(IWishlistItemService wishlistItemService)
        {
            _wishlistItemService = wishlistItemService;
        }



        [HttpPost("{productId}")]
        public async Task<IActionResult> AddWishlistItem([FromRoute] int productId)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });


            try
            {

                var result = await _wishlistItemService.AddWhishlistItemAsync(productId, userId);
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

                var result = await _wishlistItemService.RemoveWhishlistItemAsync(productId, userId);


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
