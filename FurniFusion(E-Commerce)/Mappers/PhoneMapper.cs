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

    }
}
