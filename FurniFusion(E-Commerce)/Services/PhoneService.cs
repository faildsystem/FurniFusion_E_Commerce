using FurniFusion.Data;
using FurniFusion.Models;
using FurniFusion.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class PhoneService : IPhoneService
    {
        private readonly FurniFusionDbContext _context;

        public PhoneService(FurniFusionDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<UserPhoneNumber>> AddPhoneAsync(UserPhoneNumber phone, string userId)
        {
            var existingPhone = await _context.UserPhoneNumbers
                              .FirstOrDefaultAsync(p => p.PhoneNumber == phone.PhoneNumber && p.UserId == userId);

            if (existingPhone != null)
                return ServiceResult<UserPhoneNumber>.ErrorResult("The phone number already exists for this user.", 400);

            var createdPhone = await _context.UserPhoneNumbers.AddAsync(phone);
            await _context.SaveChangesAsync();

            return ServiceResult<UserPhoneNumber>.SuccessResult(createdPhone.Entity, "Phone added successfully.");
        }

        public async Task<ServiceResult<bool>> DeletePhoneAsync(string phoneNumber, string userId)
        {
            var phone = await _context.UserPhoneNumbers.FirstOrDefaultAsync(p => p.UserId == userId && p.PhoneNumber == phoneNumber);

            if (phone == null)
                return ServiceResult<bool>.ErrorResult("Phone number not found", 404);

            _context.UserPhoneNumbers.Remove(phone);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true, "Phone number deleted successfully.");
        }

        public async Task<ServiceResult<List<UserPhoneNumber>>> GetAllPhoneByUserIdAsync(string userId)
        {
            var phones = await _context.UserPhoneNumbers.Where(p => p.UserId == userId).ToListAsync();

            return ServiceResult<List<UserPhoneNumber>>.SuccessResult(phones, "Phone numbers retrieved successfully.");
        }

        public async Task<ServiceResult<UserPhoneNumber?>> GetPhoneAsync(string phoneNumber, string userId)
        {
            var phone = await _context.UserPhoneNumbers.FirstOrDefaultAsync(p => p.UserId == userId && p.PhoneNumber == phoneNumber);

            if (phone == null)
                return ServiceResult<UserPhoneNumber?>.ErrorResult("Phone number not found", 404);

            return ServiceResult<UserPhoneNumber?>.SuccessResult(phone, "Phone number retrieved successfully.");
        }
    }
}
