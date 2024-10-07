using FurniFusion.Models;
using FurniFusion.Dtos.Review;

namespace FurniFusion.Interfaces
{
    public interface IReviewService
    {

        Task<ProductReview?> AddReviewAsync(ProductReview review);

        Task<List<ProductReview>?> GetProductReviewsAsync(int productId);

        Task<bool> DeleteReviewAsync(int reviewId, string userId, bool isAdmin);

        Task<ProductReview?> GetReviewAsync(int reviewId);

        Task<ProductReview?> UpdateReviewasync(UpdateReviewDto UpdateReviewDto);
    }
}
