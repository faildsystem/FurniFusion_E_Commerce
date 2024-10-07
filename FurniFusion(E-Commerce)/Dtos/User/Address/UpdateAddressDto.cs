using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.Profile.Address
{
    public class UpdateAddressDto
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        private DateTime UpdatedAt { get; set; }

        public bool? IsPrimaryAddress { get; set; }
    }
}

