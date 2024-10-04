using FurniFusion.Models;
using FurniFusion_E_Commerce_.Dtos.Review;

namespace FurniFusion_E_Commerce_.Interfaces
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
