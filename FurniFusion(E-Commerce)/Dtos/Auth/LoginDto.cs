// Ignore Spelling: Username Furni Dto

using System.ComponentModel.DataAnnotations;

namespace FurniFusion
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
