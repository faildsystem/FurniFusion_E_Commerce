using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin.User
{
    public class DeleteUserDto
    {
        [Required]
        public string? UserEmail { get; set; }
    }
}