using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.Profile.Address
{
    public class AddAddressDto
    {
        [Required]
        public string? Country { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public string? Street { get; set; }
        [Required]
        public string? PostalCode { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsPrimaryAddress { get; set; }

    }
}
