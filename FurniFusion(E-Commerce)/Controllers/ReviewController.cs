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
                var review = await _reviewService.GetReviewAsync(reviewId);

                if (review == null)
                    return NotFound(new { message = "Review not found" });

                return Ok(review.ToReviewDto());

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
                var reviews = await _reviewService.GetProductReviewsAsync(productId);

                if (reviews == null)
                    return NotFound(new { message = "Product not found" });

                return Ok(reviews.Select(r => r.ToReviewDto()));

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
                var CreatedReview = await _reviewService.AddReviewAsync(review);

                if (CreatedReview == null)
                    return BadRequest(new { message = "Failed to add review" });

                return CreatedAtAction(nameof(AddReview), new { id = CreatedReview.ReviewId }, CreatedReview.ToReviewDto());
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
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
                // Proceed with deletion (you'll need to implement this in your service)
                var isDeleted = await _reviewService.DeleteReviewAsync(reviewId, userId, isAdmin);

                if (!isDeleted)
                    return NotFound(new { message = "Review not found" });

                return Ok(new { message = "Review deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
