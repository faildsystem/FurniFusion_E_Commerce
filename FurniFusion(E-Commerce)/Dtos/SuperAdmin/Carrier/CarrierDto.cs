namespace FurniFusion.Dtos.SuperAdmin.Carrier
{
    public class CarrierDto
    {
        public int CarrierId { get; set; }

        public string CarrierName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string? Website { get; set; }

        public string Address { get; set; } = null!;

        public string? ShippingApi { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
