using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin
{
    public class DeleteUserDto
    {
        [Required]
        public string? UserEmail { get; set; }
    }
}