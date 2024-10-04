using FurniFusion.Models;
using FurniFusion.Dtos.Profile.Phone;

namespace FurniFusion.Interfaces
{
    public interface IPhoneService
    {
        //Task<UserPhoneNumber?> GetPhonedAsync(int phoneId, string userId);

        Task<List<UserPhoneNumber>> GetAllPhoneByUserIdAsync(string userId);

        Task<UserPhoneNumber> AddPhoneAsync(UserPhoneNumber phone, string userId);

        Task<UserPhoneNumber?> UpdatePhoneAsync(UpdatePhoneDto phoneDto, string userId);

        Task<bool> DeletePhoneAsync(string phoneNumber, string userId);



    }
}
