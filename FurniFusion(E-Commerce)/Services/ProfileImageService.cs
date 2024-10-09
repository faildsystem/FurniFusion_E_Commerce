using FurniFusion.Data;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class ProfileImageService : IProfileImageService
    {

        private readonly FurniFusionDbContext _context;


        public ProfileImageService(FurniFusionDbContext context)
        {
            _context = context;
        }


        private async Task<string?> SaveNewProfileImage(string userId, string imageUrl)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            if (user.ImageUrl != null)
                 RemoveProfileImage(user.ImageUrl);

            user.ImageUrl = imageUrl;

            await _context.SaveChangesAsync();

            return imageUrl;
        }

        private void RemoveProfileImage(string ImageUrl)
        {
            if (File.Exists(ImageUrl))
            {
               File.Delete(ImageUrl);
            }
        }

        public async Task<ServiceResult<string>> UploadProfileImageAsync(IFormFile profileImage, string userId)
        {
            var extension = Path.GetExtension(profileImage.FileName).ToLower();

            // Generate a unique file name to avoid collisions
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var folderPath = Path.Combine("wwwroot", "images", "profile");

            // Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, uniqueFileName);

            // Save the file to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(fileStream);
            }

            // Example: save the URL to the user's profile in the database
            var profileImageUrl = await SaveNewProfileImage(userId, filePath);

            if (profileImageUrl == null)
                return ServiceResult<string>.ErrorResult("User not found", StatusCodes.Status404NotFound);

            return ServiceResult<string>.SuccessResult(profileImageUrl);

        }


    }
}
