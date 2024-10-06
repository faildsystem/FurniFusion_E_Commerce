using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface IPhoneService
    {
        Task<ServiceResult<UserPhoneNumber?>> GetPhoneAsync(string phoneNumber, string userId);

        Task<ServiceResult<List<UserPhoneNumber>>> GetAllPhoneByUserIdAsync(string userId);

        Task<ServiceResult<UserPhoneNumber>> AddPhoneAsync(UserPhoneNumber phone, string userId);

        Task<ServiceResult<bool>> DeletePhoneAsync(string phoneNumber, string userId);
    }
}
