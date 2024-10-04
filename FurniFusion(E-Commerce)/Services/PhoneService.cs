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

            var existingPhone = await _context.UserPhoneNumbers
                              .FirstOrDefaultAsync(p => p.PhoneNumber == phone.PhoneNumber && p.UserId == userId);

            if (existingPhone != null)
                throw new Exception("The phone number already exists for this user.");

            var createdPhone = await _context.UserPhoneNumbers.AddAsync(phone);

            await _context.SaveChangesAsync();
            return createdPhone.Entity;

        }

        public async Task<bool> DeletePhoneAsync(string phoneNumber, string userId)
        {

            var phone = await _context.UserPhoneNumbers.FirstOrDefaultAsync(p => p.UserId == userId && p.PhoneNumber == phoneNumber);

            if (phone == null)
                return false;

            _context.UserPhoneNumbers.Remove(phone);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<List<UserPhoneNumber>> GetAllPhoneByUserIdAsync(string userId)
        {

            var phones = await _context.UserPhoneNumbers.Where(p => p.UserId == userId).ToListAsync();
            return phones ?? new List<UserPhoneNumber>();

        }

        public async Task<UserPhoneNumber?> GetPhoneAsync(string phoneNumber, string userId)
        {

            var phone = await _context.UserPhoneNumbers.FirstOrDefaultAsync(p => p.UserId == userId && p.PhoneNumber == phoneNumber);

            if (phone == null)
                return null;

            return phone;

        }

        //public async Task<UserPhoneNumber?> UpdatePhoneAsync(string phoneNumber, string userId, UpdatePhoneDto updatePhoneDto)
        //{

        //    var wantedPhone = await _context.UserPhoneNumbers.FirstOrDefaultAsync(p => p.UserId == userId && p.PhoneNumber == phoneNumber);

        //    if (wantedPhone == null)
        //        return null;

        //    wantedPhone.PhoneNumber = updatePhoneDto.PhoneNumber ;
        //    wantedPhone.UpdatedAt = DateTime.Now;

        //    await _context.SaveChangesAsync();
        //    return wantedPhone;

        //}
    }
}
