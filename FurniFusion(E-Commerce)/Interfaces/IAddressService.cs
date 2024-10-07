using FurniFusion.Models;
using FurniFusion.Dtos.Profile.Address;

namespace FurniFusion.Interfaces
{
    public interface IAddressService
    {
        Task<ServiceResult<UserAddress?>> AddAddressAsync(UserAddress address, string userId);

        Task<ServiceResult<UserAddress?>> UpdateAddressAsync(UpdateAddressDto addressDto, int addressId, string userId);

        Task<ServiceResult<bool>> DeleteAddressAsync(int addressId, string userId);

        Task<ServiceResult<UserAddress?>> GetAddressAsync(int addressId, string userId);

        Task<ServiceResult<List<UserAddress>>> GetAllAddressesAsync(string userId);
    }

}


