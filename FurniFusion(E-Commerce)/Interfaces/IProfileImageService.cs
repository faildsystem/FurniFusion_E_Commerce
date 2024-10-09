using FurniFusion.Models;
using Microsoft.AspNetCore.Mvc;

namespace FurniFusion.Interfaces
{
    public interface IProfileImageService
    {

        Task<ServiceResult<string>> UploadProfileImageAsync(IFormFile profileImage, string userId);

        

    }
}
