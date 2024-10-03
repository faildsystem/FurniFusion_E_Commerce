using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos
{
    public class ResetPasswordDto
    {

        [Required]
        public string? NewPassword { get; set; }

        [Compare("NewPassword")]
        public string? ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Token { get; set; }
    }
}
