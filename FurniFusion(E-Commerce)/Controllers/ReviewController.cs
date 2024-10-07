using FurniFusion.Dtos.Review;
using FurniFusion.Interfaces;
using FurniFusion.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReviewController : ControllerBase
    {
        // Inject your services, like a ReviewService to interact with the database
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }


        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetReview([FromRoute] int reviewId)
        {
            try
            {
                var result = await _reviewService.GetReviewAsync(reviewId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data!.ToReviewDto());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpGet("{productId}")]
        public async Task<IActionResult> GetReviews([FromRoute] int productId)
        {
            try
            {
                var result = await _reviewService.GetProductReviewsAsync(productId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data.Select(r => r.ToReviewDto()));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("{productId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> AddReview([FromRoute] int productId, [FromBody] CreateReviewDto reviewDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            var review = reviewDto.ToProductReview(userId, productId);

            try
            {
                var result = await _reviewService.AddReviewAsync(review);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return CreatedAtAction(nameof(AddReview), new { id = result.Data!.ReviewId }, result.Data.ToReviewDto());
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPut("{reviewId}")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> UpdateReview([FromRoute] int reviewId, [FromBody] UpdateReviewDto reviewDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ModelState);

            try
            {
                var result = await _reviewService.UpdateReviewasync(reviewDto, reviewId, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data!.ToReviewDto());
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "user,superAdmin")]
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            // Get the current user's ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Usually the user ID is in the 'sub' or 'NameIdentifier' claim

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ModelState);

            var isAdmin = User.IsInRole("superAdmin");

            try
            {

                var result = await _reviewService.DeleteReviewAsync(reviewId, userId, isAdmin);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = "Review deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
