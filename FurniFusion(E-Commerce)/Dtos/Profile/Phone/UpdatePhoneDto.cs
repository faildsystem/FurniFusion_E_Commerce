using System.ComponentModel.DataAnnotations;

namespace FurniFusion_E_Commerce_.Dtos.Profile.Phone
{
    public class UpdatePhoneDto
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
    }


}
