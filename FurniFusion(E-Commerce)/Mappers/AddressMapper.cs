using FurniFusion.Models;
using FurniFusion_E_Commerce_.Dtos.Profile.Address;

namespace FurniFusion_E_Commerce_.Mappers
{
    public static class AddressMapper
    {
        public static AddressDto ToAddressDto(this UserAddress address)
        {
            return new AddressDto
            {
                //AddressId = address.AddressId,
                Country = address.Country,
                City = address.City,
                State = address.State,
                Street = address.Street,
                PostalCode = address.PostalCode,
                IsPrimaryAddress = address.IsPrimaryAddress
            };
        }
    }
}
