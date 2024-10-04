using FurniFusion.Models;
using FurniFusion.Dtos.Profile.Address;

namespace FurniFusion.Interfaces
{
    public interface IAddressService
    {
        Task<UserAddress?> AddAddressAsync(UserAddress address, string userId);

        Task<UserAddress?> UpdateAddressAsync(UpdateAddressDto addressDto, string userId);

        Task<bool> DeleteAddressAsync(int addressId, string userId);
        Task<UserAddress?> GetAddressAsync(int addressId, string userId);

        Task<List<UserAddress>> GetAllAddressesAsync(string userId);

    }
}
