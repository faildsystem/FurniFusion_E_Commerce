using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos
{
    public class SendResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        //[Required]
        //public string? ClientURI { get; set; }

    }
}
