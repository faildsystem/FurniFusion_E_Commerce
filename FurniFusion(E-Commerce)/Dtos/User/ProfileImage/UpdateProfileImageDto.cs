using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using FurniFusion.Attributes;

namespace FurniFusion.Dtos.User.ProfileImage
{
    public class UpdateProfileImageDto
    {


        [Required]
        [MaxFileSize]
        [AllowedFileExtensions]
        public IFormFile? ProfileImage { get; set; }
    }
}
