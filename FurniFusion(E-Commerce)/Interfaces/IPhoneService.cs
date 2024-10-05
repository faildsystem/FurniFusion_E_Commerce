using FurniFusion.Models;
using FurniFusion.Dtos.Profile.Phone;

namespace FurniFusion.Interfaces
{
    public interface IPhoneService
    {
        Task<UserPhoneNumber?> GetPhoneAsync(string phoneNumber, string userId);

        Task<List<UserPhoneNumber>> GetAllPhoneByUserIdAsync(string userId);

        Task<UserPhoneNumber> AddPhoneAsync(UserPhoneNumber phone, string userId);

        Task<bool> DeletePhoneAsync(string phoneNumber, string userId);

        //Task<UserPhoneNumber?> UpdatePhoneAsync(string phoneNumber, string userId, UpdatePhoneDto updatePhoneDto);


    }
}
