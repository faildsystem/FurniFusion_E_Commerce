using FurniFusion.Models;
using FurniFusion_E_Commerce_.Dtos.Profile.Phone;

namespace FurniFusion_E_Commerce_.Mappers
{
    public static class PhoneMapper
    {
        public static PhoneDto ToPhoneDto(this UserPhoneNumber phone)
        {
            return new PhoneDto
            {
                PhoneNumber = phone.PhoneNumber
            };
        }

        public static UserPhoneNumber ToUserPhoneNumber(this CreatePhoneDto phoneDto, string userId)
        {
            return new UserPhoneNumber
            {
                PhoneNumber = phoneDto.PhoneNumber,
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

    }
}
