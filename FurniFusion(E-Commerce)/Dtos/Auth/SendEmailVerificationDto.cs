using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.Auth
{
    public class SendEmailVerificationDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

    }
}
