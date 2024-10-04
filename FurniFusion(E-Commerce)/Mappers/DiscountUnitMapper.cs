using FurniFusion.Dtos.ProductManager.Discount;
using FurniFusion.Models;

namespace FurniFusion.Mappers
{
    public static class DiscountUnitMapper
    {
        public static DiscountUnitDto ToDiscountUnitDto(this DiscountUnit unit)
        {
            return new DiscountUnitDto
            {
                UnitId = unit.UnitId,
                UnitName = unit.UnitName
            };
        }
    }
}
