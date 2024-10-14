using FurniFusion.Dtos.Order;
using FurniFusion.Models;

namespace FurniFusion.Mappers
{
    public static class ShippingMapper
    {

        public static ShippingDto ToShippingDto(this Shipping shipping)
        {

            return new ShippingDto
            {
                ShippingMethod = shipping.ShippingMethod,

                ShippingCost = shipping.ShippingCost,

                ShippingDate = shipping.ShippingDate,

                ShippingStatus = shipping.ShippingStatus!.StatusName

            };

        }
    }
}
