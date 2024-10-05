using FurniFusion.Models;
using FurniFusion.Queries;
using FurniFusion.Dtos.ProductManager.Discount;

namespace FurniFusion.Interfaces
{
    public interface IDiscountService
    {
        // Discount
        Task<List<Discount>> GetAllDiscountsAsync(DiscountFilter filter);
        Task<Discount> GetDiscountByIdAsync(int id);
        Task<Discount> CreateDiscountAsync(CreateDiscountDto discountDto, string creatorId);
        Task<Discount> UpdateDiscountAsync(UpdateDiscountDto discountDto, string updatorId);
        Task DeleteDiscountAsync(int id);
        //Task<bool> IsDiscountCodeExistsAsync(string code);
        //Task ActivateDiscountAsync(int id);
        //Task DeactivateDiscountAsync(int id);
        //Task<DiscountUnit> GetDiscountUnitByIdAsync(int id);


        // Discount Unit
        Task<DiscountUnit> CreateDiscountUnitAsync(CreateDiscountUnitDto discountUnitDto);
        Task<List<DiscountUnit>> GetAllDiscountUnitsAsync();
        Task<DiscountUnit> UpdateDiscountUnitAsync(UpdateDiscountUnitDto discountUnitDto);
        Task DeleteDiscountUnitAsync(int id);

    }
}
