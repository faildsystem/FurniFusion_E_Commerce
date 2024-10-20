using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin.Carrier
{
    public class CreateCarrierDto
    {
        public string CarrierName { get; set; } = null!;

        [EmailAddress]
        public string Email { get; set; } = null!;

        [Phone]
        public string Phone { get; set; } = null!;

        public string? Website { get; set; }

        public string Address { get; set; } = null!;

        public string? ShippingApi { get; set; }

        public bool? IsActive { get; set; }
    }
}
