using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin.Carrier
{
    public class DeleteCarrierDto
    {
        public int? CarrierId { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }
    }
}
