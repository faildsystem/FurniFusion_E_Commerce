using FurniFusion.Data;
using FurniFusion.Models;
using FurniFusion_E_Commerce_.Dtos.Profile.Phone;
using FurniFusion_E_Commerce_.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion_E_Commerce_.Services
{
    public class PhoneService : IPhoneService
    {
        private readonly FurniFusionDbContext _context;

        public PhoneService(FurniFusionDbContext context)
        {
            _context = context;
        }

        public async Task<UserPhoneNumber> AddPhoneAsync(UserPhoneNumber phone, string userId)
        {
            try
            {
                var existingPhone = await _context.UserPhoneNumbers
                                  .FirstOrDefaultAsync(p => p.PhoneNumber == phone.PhoneNumber && p.UserId == userId);

                if (existingPhone != null)
                    throw new Exception("The phone number already exists for this user.");

                var createdPhone = await _context.UserPhoneNumbers.AddAsync(phone);

                await _context.SaveChangesAsync();
                return createdPhone.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the phone number.", ex);
            }
        }

        public async Task<bool> DeletePhoneAsync(string phoneNumber, string userId)
        {
            try
            {
                var phone = await _context.UserPhoneNumbers.FirstOrDefaultAsync(p => p.UserId == userId && p.PhoneNumber == phoneNumber);

                if (phone == null)
                    return false;

                _context.UserPhoneNumbers.Remove(phone);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the phone number.", ex);
            }
        }

        public async Task<List<UserPhoneNumber>> GetAllPhoneByUserIdAsync(string userId)
        {
            try
            {
                var phones = await _context.UserPhoneNumbers.Where(p => p.UserId == userId).ToListAsync();
                return phones ?? new List<UserPhoneNumber>();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting all phone numbers.", ex);
            }
        }

        //public async Task<UserPhoneNumber?> GetPhoneAsync(int phoneId, string userId)
        //{
        //    try
        //    {
        //        var phone = await _context.UserPhoneNumbers.FirstOrDefaultAsync(p => p.UserId == userId && p.PhoneNumber == p);

        //        if (phone == null)
        //            return null;

        //        return phone;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("An error occurred while getting the phone number.", ex);
        //    }
        //}

        public async Task<UserPhoneNumber?> UpdatePhoneAsync(UpdatePhoneDto phoneDto, string userId)
        {
            try
            {
                var wantedPhone = await _context.UserPhoneNumbers.FirstOrDefaultAsync(p => p.UserId == userId && p.PhoneNumber == phoneDto.PhoneNumber);

                if (wantedPhone == null)
                    return null;

                wantedPhone.PhoneNumber = phoneDto.PhoneNumber;
                wantedPhone.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return wantedPhone;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the phone number.", ex);
            }
        }
    }
}
