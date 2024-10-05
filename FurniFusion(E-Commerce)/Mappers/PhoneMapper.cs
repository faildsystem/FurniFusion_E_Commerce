using FurniFusion.Models;
using FurniFusion.Dtos.Profile.Phone;

namespace FurniFusion.Mappers
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
