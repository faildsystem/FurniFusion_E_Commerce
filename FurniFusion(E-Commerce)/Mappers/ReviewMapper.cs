using FurniFusion.Models;
using FurniFusion.Dtos.Review;

namespace FurniFusion.Mappers
{
    public static class ReviewMapper
    {

        public static ReviewDto ToReviewDto(this ProductReview review)
        {
            return new ReviewDto
            {
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                UpdatedAt = review.UpdatedAt
            };
        }

        public static ProductReview ToProductReview(this CreateReviewDto reviewDto, string userId, int productId)
        {
            return new ProductReview
            {
                UserId = userId,
                ProductId = productId,
                Rating = reviewDto.Rating,
                ReviewText = reviewDto.ReviewText,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }
    }
}
