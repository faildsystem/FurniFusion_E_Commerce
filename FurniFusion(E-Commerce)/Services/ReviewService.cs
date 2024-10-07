using FurniFusion.Data;
using FurniFusion.Models;
using FurniFusion.Dtos.Review;
using FurniFusion.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class ReviewService : IReviewService
    {
        private readonly FurniFusionDbContext _context;

        public ReviewService(FurniFusionDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<ProductReview?>> AddReviewAsync(ProductReview review)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == review.ProductId);

            if (product == null)
                return ServiceResult<ProductReview?>.ErrorResult("Product not found", StatusCodes.Status404NotFound);

            var createdReview = await _context.ProductReviews.AddAsync(review);
            await _context.SaveChangesAsync();

            return ServiceResult<ProductReview?>.SuccessResult(createdReview.Entity, "Review added successfully", StatusCodes.Status201Created);
        }

        public async Task<ServiceResult<bool>> DeleteReviewAsync(int reviewId, string userId, bool isAdmin)
        {
            var review = await GetReviewAsync(reviewId);

            if (review == null || review.Data == null)
                return ServiceResult<bool>.ErrorResult("Review not found", StatusCodes.Status404NotFound);

            if (!isAdmin && review.Data.UserId != userId)
                return ServiceResult<bool>.ErrorResult("Unauthorized", StatusCodes.Status403Forbidden);

            _context.ProductReviews.Remove(review.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true, "Review deleted successfully");
        }

        public async Task<ServiceResult<ProductReview?>> GetReviewAsync(int reviewId)
        {
            var review = await _context.ProductReviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);

            if (review == null)
                return ServiceResult<ProductReview?>.ErrorResult("Review not found", StatusCodes.Status404NotFound);

            return ServiceResult<ProductReview?>.SuccessResult(review);
        }

        public async Task<ServiceResult<List<ProductReview>>> GetProductReviewsAsync(int productId)
        {
            var reviews = await _context.ProductReviews
                                        .Where(r => r.ProductId == productId)
                                        .ToListAsync();

            if (reviews == null || !reviews.Any())
                return ServiceResult<List<ProductReview>>.ErrorResult("No reviews found for this product", StatusCodes.Status404NotFound);

            return ServiceResult<List<ProductReview>>.SuccessResult(reviews);
        }

        public async Task<ServiceResult<ProductReview?>> UpdateReviewasync(UpdateReviewDto updateReviewDto, int reviewId, string userId)
        {
            var reviewResult = await GetReviewAsync(reviewId);

            if (reviewResult.Data == null)
                return ServiceResult<ProductReview?>.ErrorResult("Review not found", StatusCodes.Status404NotFound);

            var review = reviewResult.Data;

            if (review.UserId != userId)
                return ServiceResult<ProductReview?>.ErrorResult("Unauthorized", StatusCodes.Status403Forbidden);

            review.Rating = updateReviewDto.Rating ?? review.Rating;
            review.ReviewText = updateReviewDto.ReviewText ?? review.ReviewText;

            _context.ProductReviews.Update(review);
            await _context.SaveChangesAsync();

            return ServiceResult<ProductReview?>.SuccessResult(review, "Review updated successfully");
        }
    }

}
