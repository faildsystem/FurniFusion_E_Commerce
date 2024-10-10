using FurniFusion.Dtos.ProductManager.Discount;
using FurniFusion.Models;

namespace FurniFusion.Mappers
{
    public static class DiscountMapper
    {
        public static DiscountDto ToDiscountDto(this Discount discount)
        {
            return new DiscountDto
            {
                DiscountId = discount.DiscountId,
                DiscountCode = discount.DiscountCode,
                DiscountValue = discount.DiscountValue,
                DiscountUnitId = discount.DiscountUnitId,
                DiscountUnit = discount.DiscountUnit.ToDiscountUnitDto(),
                IsActive = discount.IsActive,
                ValidFrom = discount.ValidFrom,
                ValidTo = discount.ValidTo,
                MaxAmount = discount.MaxAmount,
                CreatedBy = discount.CreatedBy,
                UpdatedBy = discount.UpdatedBy,
                CreatedAt = discount.CreatedAt,
                UpdatedAt = discount.UpdatedAt,
                Products = discount.Products.Select(p => p.ToProductDto()).ToList()
            };
        }
    }
}
