using FurniFusion.Data;
using FurniFusion.Dtos.Profile.Address;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class AddressService : IAddressService
    {
        private readonly FurniFusionDbContext _context;

        public AddressService(FurniFusionDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<UserAddress?>> GetAddressAsync(int addressId, string userId)
        {
            var address = await _context.UserAddresses
                                        .FirstOrDefaultAsync(a => a.AddressId == addressId);

            if (address == null)
                return ServiceResult<UserAddress?>.ErrorResult("Address not found", StatusCodes.Status404NotFound);

            if (address.UserId != userId)
                return ServiceResult<UserAddress?>.ErrorResult("Unauthorized", StatusCodes.Status403Forbidden);

            return ServiceResult<UserAddress?>.SuccessResult(address);
        }

        public async Task<ServiceResult<List<UserAddress>>> GetAllAddressesAsync(string userId)
        {
            var addresses = await _context.UserAddresses
                                          .Where(a => a.UserId == userId)
                                          .ToListAsync();

            if (addresses.Count == 0)
                return ServiceResult<List<UserAddress>>.ErrorResult("No addresses found", StatusCodes.Status404NotFound);

            return ServiceResult<List<UserAddress>>.SuccessResult(addresses);
        }

        public async Task<ServiceResult<UserAddress?>> AddAddressAsync(UserAddress address, string userId)
        {

            var existingAddress = await _context.UserAddresses
                                                .FirstOrDefaultAsync(a => a.Street == address.Street
                                                                        && a.City == address.City
                                                                        && a.UserId == userId);

            if (existingAddress != null)
                return ServiceResult<UserAddress?>.ErrorResult("Address already exists for this user", StatusCodes.Status400BadRequest);

            if (address.IsPrimaryAddress == true)
            {
                await _context.UserAddresses
                                            .Where(a => a.UserId == userId)
                                            .ExecuteUpdateAsync(address => address.SetProperty(a => a.IsPrimaryAddress, false));

            }

            var createdAddress = await _context.UserAddresses.AddAsync(address);
            await _context.SaveChangesAsync();

            return ServiceResult<UserAddress?>.SuccessResult(createdAddress.Entity, "Address added successfully", StatusCodes.Status201Created);

        }

        public async Task<ServiceResult<UserAddress?>> UpdateAddressAsync(UpdateAddressDto addressDto, int addressId, string userId)
        {
            var address = await _context.UserAddresses.FirstOrDefaultAsync(a => a.AddressId == addressId);

            if (address == null)
                return ServiceResult<UserAddress?>.ErrorResult("Address not found", StatusCodes.Status404NotFound);

            if (address.UserId != userId)
                return ServiceResult<UserAddress?>.ErrorResult("Unauthorized", StatusCodes.Status403Forbidden);

            if (addressDto.IsPrimaryAddress == true)
            {
                await _context.UserAddresses
                    .Where(a => a.UserId == userId)
                    .ExecuteUpdateAsync(address => address.SetProperty(a => a.IsPrimaryAddress, false));
            }

            address.Country = addressDto.Country ?? address.Country;
            address.City = addressDto.City ?? address.City;
            address.State = addressDto.State ?? address.State;
            address.Street = addressDto.Street ?? address.Street;
            address.PostalCode = addressDto.PostalCode ?? address.PostalCode;
            address.IsPrimaryAddress = addressDto.IsPrimaryAddress ?? address.IsPrimaryAddress;
            address.UpdatedAt = DateTime.Now;


            await _context.SaveChangesAsync();

            return ServiceResult<UserAddress?>.SuccessResult(address);
        }

        public async Task<ServiceResult<bool>> DeleteAddressAsync(int addressId, string userId)
        {
            var address = await _context.UserAddresses
                                        .FirstOrDefaultAsync(a => a.AddressId == addressId);

            if (address == null)
                return ServiceResult<bool>.ErrorResult("Address not found", StatusCodes.Status404NotFound);

            if (address.UserId != userId)
                return ServiceResult<bool>.ErrorResult("Unauthorized", StatusCodes.Status403Forbidden);

            _context.UserAddresses.Remove(address);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }
    }

}
