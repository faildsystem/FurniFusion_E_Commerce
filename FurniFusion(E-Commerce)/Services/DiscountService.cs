using FurniFusion.Data;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using FurniFusion.Queries;
using FurniFusion.Dtos.ProductManager.Discount;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly FurniFusionDbContext _context;

        public DiscountService(FurniFusionDbContext context)
        {
            _context = context;
        }

        // Discount
        public async Task<ServiceResult<List<Discount>>> GetAllDiscountsAsync(DiscountFilter filter)
        {
            var discounts = _context.Discounts.Include(u => u.DiscountUnit).AsQueryable();

            if (filter.DiscountId > 0)
            {
                discounts = discounts.Where(x => x.DiscountId == filter.DiscountId);
            }

            if (!string.IsNullOrEmpty(filter.DiscountCode))
            {
                discounts = discounts.Where(x => x.DiscountCode == filter.DiscountCode);
            }

            if (filter.DiscountValue.HasValue)
            {
                discounts = discounts.Where(x => x.DiscountValue == filter.DiscountValue);
            }

            if (filter.DiscountUnitId.HasValue)
            {
                discounts = discounts.Where(x => x.DiscountUnitId == filter.DiscountUnitId);
            }

            if (filter.IsActive.HasValue)
            {
                discounts = discounts.Where(x => x.IsActive == filter.IsActive);
            }

            if (filter.ValidFrom.HasValue)
            {
                discounts = discounts.Where(x => x.ValidFrom == filter.ValidFrom);
            }

            if (filter.ValidTo.HasValue)
            {
                discounts = discounts.Where(x => x.ValidTo <= filter.ValidTo);
            }

            if (filter.MaxAmount.HasValue)
            {
                discounts = discounts.Where(x => x.MaxAmount == filter.MaxAmount);
            }

            if (!string.IsNullOrEmpty(filter.CreatedBy))
            {
                discounts = discounts.Where(x => x.CreatedBy == filter.CreatedBy);
            }

            if (!string.IsNullOrEmpty(filter.UpdatedBy))
            {
                discounts = discounts.Where(x => x.UpdatedBy == filter.UpdatedBy);
            }

            var skipNumber = (filter.PageNumber - 1) * filter.PageSize;

            var productsList = await discounts.Skip(skipNumber).Take(filter.PageSize).ToListAsync();

           
            var result = await discounts.Skip(skipNumber).Take(filter.PageSize).ToListAsync();

            return ServiceResult<List<Discount>>.SuccessResult(result);
        }

        public async Task<ServiceResult<Discount>> CreateDiscountAsync(CreateDiscountDto discountDto, string creatorId)
        {
            var discountUnit = await _context.DiscountUnits.FirstOrDefaultAsync(d => d.UnitId == discountDto.DiscountUnitId);

            if (discountUnit == null)
            {
                return ServiceResult<Discount>.ErrorResult("Discount unit not found", StatusCodes.Status404NotFound);
            }

            var result = await GetDiscountByCodeAsync(discountDto.DiscountCode);

            if (result != null && result.Data != null)
            {
                return ServiceResult<Discount>.ErrorResult("Discount code already exists", StatusCodes.Status409Conflict);
            }

            var createdDiscount = new Discount
            {
                DiscountCode = discountDto.DiscountCode,
                DiscountValue = discountDto.DiscountValue,
                DiscountUnitId = discountDto.DiscountUnitId,
                IsActive = discountDto.IsActive,
                ValidFrom = discountDto.ValidFrom,
                ValidTo = discountDto.ValidTo,
                MaxAmount = discountDto.MaxAmount,
                CreatedBy = creatorId,
                UpdatedBy = creatorId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
 
            await _context.Discounts.AddAsync(createdDiscount);
            await _context.SaveChangesAsync();
            
            return ServiceResult<Discount>.SuccessResult(createdDiscount, "Discount created successfully", StatusCodes.Status201Created);
        }

        public async Task<ServiceResult<Discount>> UpdateDiscountAsync(UpdateDiscountDto discountDto, string updatorId)
        {
            var result = await GetDiscountByIdAsync(discountDto.DiscountId);

            if (result == null || result.Data == null)
            {
                return ServiceResult<Discount>.ErrorResult("Discount not found", StatusCodes.Status404NotFound);
            }

            if (discountDto.DiscountCode != null)
            {
                var existingDiscount = await GetDiscountByCodeAsync(discountDto.DiscountCode);

                if (existingDiscount != null || existingDiscount.Data != null)
                {
                    return ServiceResult<Discount>.ErrorResult("Discount code already exists", StatusCodes.Status409Conflict);
                }
            }

            result.Data.DiscountCode = discountDto.DiscountCode ?? result.Data.DiscountCode;
            result.Data.DiscountValue = discountDto.DiscountValue ?? result.Data.DiscountValue;
            result.Data.DiscountUnitId = discountDto.DiscountUnitId ?? result.Data.DiscountUnitId;
            result.Data.IsActive = discountDto.IsActive ?? result.Data.IsActive;
            result.Data.ValidFrom = discountDto.ValidFrom ?? result.Data.ValidFrom;
            result.Data.ValidTo = discountDto.ValidTo ?? result.Data.ValidTo;
            result.Data.MaxAmount = discountDto.MaxAmount ?? result.Data.MaxAmount;
            result.Data.UpdatedBy = updatorId;
            result.Data.UpdatedAt = DateTime.Now;

            _context.Discounts.Update(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<Discount>.SuccessResult(result.Data, "Discount updated successfully");

        }

        public async Task<ServiceResult<bool>> DeleteDiscountAsync(int id)
        {
            var result = await GetDiscountByIdAsync(id);

            if (result == null || result.Data == null)
            {
                return ServiceResult<bool>.ErrorResult("Discount not found", StatusCodes.Status404NotFound);
            }

            _context.Discounts.Remove(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true, "Discount deleted successfully");
        }

        public async Task<ServiceResult<Discount>> GetDiscountByIdAsync(int? id)
        {
            var discount = await _context.Discounts.Include(u => u.DiscountUnit).FirstOrDefaultAsync(p => p.DiscountId == id);

            if (discount == null)
            {
                return ServiceResult<Discount>.ErrorResult("Discount not found", StatusCodes.Status404NotFound);
            }

            return ServiceResult<Discount>.SuccessResult(discount);
        }

        public async Task<ServiceResult<Discount?>> GetDiscountByCodeAsync(string code)
        {
            var discount = await _context.Discounts.Include(u => u.DiscountUnit).FirstOrDefaultAsync(p => p.DiscountCode == code);

            if (discount == null)
            {
                return ServiceResult<Discount?>.ErrorResult("Discount not found", StatusCodes.Status404NotFound);
            }

            return ServiceResult<Discount?>.SuccessResult(discount);
        }

        public async Task<ServiceResult<List<Discount>>> GetActiveDiscountsAsync()
        {
            var activeDiscounts = await _context.Discounts.Include(u => u.DiscountUnit).Where(d => d.IsActive == true).ToListAsync();

            return ServiceResult<List<Discount>>.SuccessResult(activeDiscounts);
        }

        public async Task<ServiceResult<bool>> ValidateDiscountCodeAsync(string code)
        {
            // Retrieve the discount with the provided code
            var result = await GetDiscountByCodeAsync(code);

            if (result == null || result.Data == null)
            {
                return ServiceResult<bool>.ErrorResult("Discount not found", StatusCodes.Status404NotFound);
            }
            
            if (!result.Data.IsActive.HasValue || !result.Data.IsActive.Value)
            {
                return ServiceResult<bool>.ErrorResult("Discount is not active", StatusCodes.Status400BadRequest);
            }

            if (result.Data.ValidFrom.HasValue && result.Data.ValidFrom > DateOnly.FromDateTime(DateTime.Now))
            {
                return ServiceResult<bool>.ErrorResult("Discount is not yet valid", StatusCodes.Status400BadRequest);
            }

            if (result.Data.ValidTo.HasValue && result.Data.ValidTo < DateOnly.FromDateTime(DateTime.Now))
            {
                return ServiceResult<bool>.ErrorResult("Discount has expired", StatusCodes.Status400BadRequest);
            }

           return ServiceResult<bool>.SuccessResult(true, "Discount is valid");

        }

        public async Task<ServiceResult<bool>> DeactivateDiscountAsync(int id)
        {
            var result = await GetDiscountByIdAsync(id);

            if (result == null || result.Data == null)
            {
                return ServiceResult<bool>.ErrorResult("Discount not found", StatusCodes.Status404NotFound);
            }

            result.Data.IsActive = false;
            _context.Discounts.Update(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true, "Discount deactivated successfully");
        }

        public async Task<ServiceResult<bool>> ActivateDiscountAsync(int id)
        {
            var result = await GetDiscountByIdAsync(id);

            if (result == null || result.Data == null)
            {
                return ServiceResult<bool>.ErrorResult("Discount not found", StatusCodes.Status404NotFound);
            }

            result.Data.IsActive = true;
            _context.Discounts.Update(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true, "Discount activated successfully");
        }
    }
}
