//using FurniFusion.Data;
//using FurniFusion.Models;
//using FurniFusion_E_Commerce_.Dtos.Profile.Address;
//using FurniFusion_E_Commerce_.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace FurniFusion_E_Commerce_.Services
//{
//    public class AddressService : IAddressService
//    {
//        private readonly FurniFusionDbContext _context;

//        public AddressService(FurniFusionDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<UserAddress?> GetAddressAsync(int addressId, string userId)
//        {
//            var address = await _context.UserAddresses
//                                        .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);

//            return address;
//        }

//        public async Task<List<UserAddress>> GetAllAddressesAsync(string userId)
//        {
//            return await _context.UserAddresses
//                                 .Where(a => a.UserId == userId)
//                                 .ToListAsync() ?? new List<UserAddress>();
//        }

//        public async Task<UserAddress?> AddAddressAsync(UserAddress address, string userId)
//        {
//            try
//            {
//                var existingAddress = await _context.UserAddresses
//                                                    .FirstOrDefaultAsync(a => a.Street == address.Street
//                                                                           && a.City == address.City
//                                                                           && a.UserId == userId);

//                if (existingAddress != null)
//                    throw new Exception("The address already exists for this user.");

//                var createdAddress = await _context.UserAddresses.AddAsync(address);
//                await _context.SaveChangesAsync();
//                return createdAddress.Entity;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("An error occurred while adding the address.", ex);
//            }
//        }

//        public async Task<UserAddress?> UpdateAddressAsync(UpdateAddressDto addressDto, string userId)
//        {
//            try
//            {
//                var address = await _context.UserAddresses.FirstOrDefaultAsync(a => a.AddressId == addressDto.AddressId && a.UserId == userId);

//                if (address == null)
//                    return null;

//                address.Country = addressDto.Country ?? address.Country;
//                address.City = addressDto.City ?? address.City;
//                address.State = addressDto.State ?? address.State;
//                address.Street = addressDto.Street ?? address.Street;
//                address.PostalCode = addressDto.PostalCode ?? address.PostalCode;
//                address.IsPrimaryAddress = addressDto.IsPrimaryAddress ?? address.IsPrimaryAddress;
//                address.UpdatedAt = DateTime.Now;

//                await _context.SaveChangesAsync();

//                return address;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("An error occurred while updating the address.", ex);
//            }
//        }

//        public async Task<bool> DeleteAddressAsync(int addressId, string userId)
//        {
//            var address = await _context.UserAddresses
//                                        .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);

//            if (address == null)
//                return false;

//            _context.UserAddresses.Remove(address);
//            await _context.SaveChangesAsync();

//            return true;
//        }
//    }
//}
