using FurniFusion.Models;
using FurniFusion.Dtos.ProductManager.DiscountUnit;

namespace FurniFusion.Interfaces
{
    public interface IDiscountUnitService
    {
        Task<ServiceResult<DiscountUnit>> CreateDiscountUnitAsync(CreateDiscountUnitDto discountUnitDto);
        Task<ServiceResult<DiscountUnit>> GetDiscountUnitByIdAsync(int? id);
        Task<ServiceResult<List<DiscountUnit>>> GetAllDiscountUnitsAsync();
        Task<ServiceResult<DiscountUnit>> UpdateDiscountUnitAsync(UpdateDiscountUnitDto discountUnitDto);
        Task<ServiceResult<bool>> DeleteDiscountUnitAsync(int id);
    }
}
