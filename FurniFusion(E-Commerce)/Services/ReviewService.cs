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

        public async Task<ProductReview?> AddReviewAsync(ProductReview review)
        {

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == review.ProductId);

            if (product == null)
                return null;

            var createdReview = await _context.ProductReviews.AddAsync(review);

            await _context.SaveChangesAsync();

            return createdReview.Entity;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId, string userId, bool isAdmin)
        {
            var review = await GetReviewAsync(reviewId);

            if (review == null)
                return false;

            if (!isAdmin && review.UserId != userId)
                return false;

            _context.ProductReviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductReview?> GetReviewAsync(int reviewId)
        {
            var review = await _context.ProductReviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);

            if(review == null)
                return null;

            return review;

        }

        public async Task<List<ProductReview>?> GetProductReviewsAsync(int productId)
        {
            var reviews = await _context.ProductReviews
                                        .Where(r => r.ProductId == productId)
                                        .ToListAsync();

            if(reviews == null)
                return null;

            return reviews ;
        }

        public Task<ProductReview?> UpdateReviewasync(UpdateReviewDto UpdateReviewDto)
        {
            throw new NotImplementedException();
        }
    }
}
