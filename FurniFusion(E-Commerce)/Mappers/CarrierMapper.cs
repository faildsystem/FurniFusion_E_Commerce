using FurniFusion.Dtos.SuperAdmin.Carrier;
using FurniFusion.Models;

namespace FurniFusion.Mappers
{
    public static class CarrierMapper
    {
        public static CarrierDto ToCarrierDto(this Carrier carrier)
        {
            return new CarrierDto
            {
                CarrierId = carrier.CarrierId,
                CarrierName = carrier.CarrierName,
                Email = carrier.Email,
                Phone = carrier.Phone,
                Website = carrier.Website,
                Address = carrier.Address,
                ShippingApi = carrier.ShippingApi,
                IsActive = carrier.IsActive,
                CreatedAt = carrier.CreatedAt,
                UpdatedAt = carrier.UpdatedAt
            };
        }   
    }
}
