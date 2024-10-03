using System.ComponentModel.DataAnnotations;

namespace FurniFusion_E_Commerce_.Dtos.Profile.Address
{
    public class AddressDto
    {
        public int? AddressId { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public bool IsPrimaryAddress { get; set; }
    }
}
