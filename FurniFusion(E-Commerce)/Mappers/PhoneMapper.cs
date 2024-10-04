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

    }
}
