using FurniFusion.Models;
using FurniFusion.Dtos.Review;

namespace FurniFusion.Interfaces
{
    public interface IReviewService
    {
        Task<ServiceResult<ProductReview?>> AddReviewAsync(ProductReview review);
        Task<ServiceResult<List<ProductReview>>> GetProductReviewsAsync(int productId);
        Task<ServiceResult<bool>> DeleteReviewAsync(int reviewId, string userId, bool isAdmin);
        Task<ServiceResult<ProductReview?>> GetReviewAsync(int reviewId);
        Task<ServiceResult<ProductReview?>> UpdateReviewasync(UpdateReviewDto updateReviewDto, int reviewId, string userId);
    }

}
