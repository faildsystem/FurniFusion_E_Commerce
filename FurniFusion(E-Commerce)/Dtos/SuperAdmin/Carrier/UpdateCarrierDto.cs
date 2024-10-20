using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin.Carrier
{
    public class UpdateCarrierDto
    {
        [Required]
        public int CarrierId { get; set; }

        public string? CarrierName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? Website { get; set; }

        public string? Address { get; set; }

        public string? ShippingApi { get; set; }

        public bool? IsActive { get; set; }
    }
}
