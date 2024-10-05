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
        public async Task<List<Discount>> GetAllDiscountsAsync(DiscountFilter filter)
        {
            var discounts = _context.Discounts.AsQueryable();

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

           
            return await discounts.Skip(skipNumber).Take(filter.PageSize).ToListAsync();
        }

        public async Task<Discount> GetDiscountByIdAsync(int id)
        {
            var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == id);

            if (discount == null)
            {
                throw new Exception($"Discount with ID: {id} not found.");
            }

            return discount;
        }

        public async Task<Discount> CreateDiscountAsync(CreateDiscountDto discountDto, string creatorId)
        {
            var discountUnit = await _context.DiscountUnits.FirstOrDefaultAsync(d => d.UnitId == discountDto.DiscountUnitId);

            if (discountUnit == null)
            {
                throw new Exception("Discount unit not found");
            }

            var discount = await _context.Discounts
        .FirstOrDefaultAsync(d => d.DiscountCode == discountDto.DiscountCode);

            if (discount != null)
            {
                throw new Exception("Discount with the same code already exists");
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
            return createdDiscount;
        }

        public async Task<Discount> UpdateDiscountAsync(UpdateDiscountDto discountDto, string updatorId)
        {
            var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == discountDto.DiscountId);

            if (discount == null)
            {
                throw new Exception("Discount not found");
            }

            discount.DiscountCode = discountDto.DiscountCode ?? discount.DiscountCode;
            discount.DiscountValue = discountDto.DiscountValue ?? discount.DiscountValue;
            discount.DiscountUnitId = discountDto.DiscountUnitId ?? discount.DiscountUnitId;
            discount.IsActive = discountDto.IsActive ?? discount.IsActive;
            discount.ValidFrom = discountDto.ValidFrom ?? discount.ValidFrom;
            discount.ValidTo = discountDto.ValidTo ?? discount.ValidTo;
            discount.MaxAmount = discountDto.MaxAmount ?? discount.MaxAmount;
            discount.UpdatedBy = updatorId;
            discount.UpdatedAt = DateTime.Now;

            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();

            return discount;

        }

        public async Task DeleteDiscountAsync(int id)
        {
            var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == id);
            if (discount == null)
            {
                throw new Exception("Discount not found");
            }

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
        }

        // Discount Unit
        public async Task<DiscountUnit> CreateDiscountUnitAsync(CreateDiscountUnitDto discountUnitDto)
        {
            var discountUnit = await _context.DiscountUnits
                .FirstOrDefaultAsync(d => d.UnitName == discountUnitDto.UnitName);

            if (discountUnit != null)
            {
                throw new Exception("Discount unit with the same name already exists");
            }

            var newDiscountUnit = new DiscountUnit
            {
                UnitName = discountUnitDto.UnitName
            };

            await _context.DiscountUnits.AddAsync(newDiscountUnit);
            await _context.SaveChangesAsync();

            return newDiscountUnit;
        }

        public async Task<List<DiscountUnit>> GetAllDiscountUnitsAsync()
        {
            var discountUnits = await _context.DiscountUnits.ToListAsync();
            return discountUnits;
        }

        public async Task<DiscountUnit> UpdateDiscountUnitAsync(UpdateDiscountUnitDto discountUnitDto)
        {
            var discountUnit = await _context.DiscountUnits.FirstOrDefaultAsync(d => d.UnitId == discountUnitDto.UnitId);

            if (discountUnit == null)
            {
                throw new Exception("Discount unit not found");
            }

            discountUnit.UnitName = discountUnitDto.UnitName;

            _context.DiscountUnits.Update(discountUnit);
            await _context.SaveChangesAsync();
            return discountUnit;
        }

        public async Task DeleteDiscountUnitAsync(int id)
        {
            var discountUnit = await _context.DiscountUnits.FirstOrDefaultAsync(d => d.UnitId == id);

            if (discountUnit == null)
            {
                throw new Exception("Discount unit not found");
            }

            _context.DiscountUnits.Remove(discountUnit);
            await _context.SaveChangesAsync();
        }
    }
}
