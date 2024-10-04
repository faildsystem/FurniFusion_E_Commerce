using FurniFusion.Models;
using FurniFusion.Queries;
using FurniFusion.Dtos.ProductManager.Discount;

namespace FurniFusion.Interfaces
{
    public interface IDiscountService
    {
        // Discount
        Task<List<Discount>> GetAllDiscountsAsync(DiscountFilter filter);
        Task<Discount> CreateDiscountAsync(CreateDiscountDto discountDto, string creatorId);
        Task<Discount> UpdateDiscountAsync(UpdateDiscountDto discountDto, string updatorId);
        Task DeleteDiscountAsync(int id);

        // Discount Unit
        Task<DiscountUnit> CreateDiscountUnitAsync(CreateDiscountUnitDto discountUnitDto);
        Task<List<DiscountUnit>> GetAllDiscountUnitsAsync();
        Task<DiscountUnit> UpdateDiscountUnitAsync(UpdateDiscountUnitDto discountUnitDto);
        Task DeleteDiscountUnitAsync(int id);

    }
}
