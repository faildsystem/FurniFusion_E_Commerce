using FurniFusion.Models;
using FurniFusion.Queries;
using FurniFusion.Dtos.ProductManager.Discount;

namespace FurniFusion.Interfaces
{
    public interface IDiscountService
    {
        // Discount
        Task<ServiceResult<List<Discount>>> GetAllDiscountsAsync(DiscountFilter filter);
        Task<ServiceResult<Discount>> GetDiscountByIdAsync(int? id);
        Task<ServiceResult<Discount>> CreateDiscountAsync(CreateDiscountDto discountDto, string creatorId);
        Task<ServiceResult<Discount>> UpdateDiscountAsync(UpdateDiscountDto discountDto, string updatorId);
        Task<ServiceResult<bool>> DeleteDiscountAsync(int id);
        Task<ServiceResult<Discount?>> GetDiscountByCodeAsync(string code);
        Task<ServiceResult<List<Discount>>> GetActiveDiscountsAsync();
        Task<ServiceResult<bool>> ValidateDiscountCodeAsync(string code);
        Task<ServiceResult<bool>> DeactivateDiscountAsync(int id);
        Task<ServiceResult<bool>> ActivateDiscountAsync(int id);
    }
}
