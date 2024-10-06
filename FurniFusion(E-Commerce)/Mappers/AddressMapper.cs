using FurniFusion.Models;
using FurniFusion.Dtos.Profile.Address;

namespace FurniFusion.Mappers
{
    public static class AddressMapper
    {
        public static AddressDto ToAddressDto(this UserAddress address)
        {
            return new AddressDto
            {
                AddressId = address.AddressId,
                Country = address.Country,
                City = address.City,
                State = address.State,
                Street = address.Street,
                PostalCode = address.PostalCode,
                IsPrimaryAddress = address.IsPrimaryAddress
            };
        }

        public static UserAddress ToUserAddress(this AddAddressDto address, string userId)
        {
            return new UserAddress
            {
                UserId = userId,
                Country = address.Country!,
                City = address.City!,
                State = address.State!,
                Street = address.Street!,
                PostalCode = address.PostalCode!,
                IsPrimaryAddress = address.IsPrimaryAddress,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }
    }
}
