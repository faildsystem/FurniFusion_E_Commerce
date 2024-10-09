using FurniFusion.Data;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.EntityFrameworkCore;
using FurniFusion.Dtos.ProductManager.DiscountUnit;

namespace FurniFusion.Services
{
    public class DiscountUnitService : IDiscountUnitService
    {
        private readonly FurniFusionDbContext _context;

        public DiscountUnitService(FurniFusionDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<DiscountUnit>> CreateDiscountUnitAsync(CreateDiscountUnitDto discountUnitDto)
        {
            var discountUnit = await _context.DiscountUnits
                .FirstOrDefaultAsync(d => d.UnitName == discountUnitDto.UnitName);

            if (discountUnit != null)
            {
                return ServiceResult<DiscountUnit>.ErrorResult("Discount unit already exists", StatusCodes.Status409Conflict);
            }

            var newDiscountUnit = new DiscountUnit
            {
                UnitName = discountUnitDto.UnitName
            };

            await _context.DiscountUnits.AddAsync(newDiscountUnit);
            await _context.SaveChangesAsync();

            return ServiceResult<DiscountUnit>.SuccessResult(newDiscountUnit, "Discount unit created successfully", StatusCodes.Status201Created);
        }

        public async Task<ServiceResult<List<DiscountUnit>>> GetAllDiscountUnitsAsync()
        {
            var discountUnits = await _context.DiscountUnits.ToListAsync();

            return ServiceResult<List<DiscountUnit>>.SuccessResult(discountUnits);
        }

        public async Task<ServiceResult<DiscountUnit>> GetDiscountUnitByIdAsync(int? id)
        {
            var discountUnit = await _context.DiscountUnits.FirstOrDefaultAsync(d => d.UnitId == id);

            if (discountUnit == null)
            {
                return ServiceResult<DiscountUnit>.ErrorResult("Discount unit not found", StatusCodes.Status404NotFound);
            }

            return ServiceResult<DiscountUnit>.SuccessResult(discountUnit); 
        }

        public async Task<ServiceResult<DiscountUnit>> UpdateDiscountUnitAsync(UpdateDiscountUnitDto discountUnitDto)
        {
            var result = await GetDiscountUnitByIdAsync(discountUnitDto.UnitId);

            if (result == null || result.Data == null)
            {
                return ServiceResult<DiscountUnit>.ErrorResult("Discount unit not found", StatusCodes.Status404NotFound);
            }
            
            var discountUnit = await _context.DiscountUnits
                .FirstOrDefaultAsync(d => d.UnitName == discountUnitDto.UnitName);

            if (discountUnit != null)
            {
                return ServiceResult<DiscountUnit>.ErrorResult("Discount unit already exists", StatusCodes.Status409Conflict);
            }

            result.Data.UnitName = discountUnitDto.UnitName;

            _context.DiscountUnits.Update(result.Data);
            await _context.SaveChangesAsync();
            
            return ServiceResult<DiscountUnit>.SuccessResult(result.Data, "Discount unit updated successfully");
        }

        public async Task<ServiceResult<bool>> DeleteDiscountUnitAsync(int id)
        {
            var result = await GetDiscountUnitByIdAsync(id);

            if (result == null || result.Data == null)
            {
                return ServiceResult<bool>.ErrorResult("Discount unit not found", StatusCodes.Status404NotFound);
            }

            _context.DiscountUnits.Remove(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true, "Discount unit deleted successfully");
        }

    }
}
