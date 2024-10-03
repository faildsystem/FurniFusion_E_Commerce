using System.ComponentModel.DataAnnotations;

namespace FurniFusion_E_Commerce_.Dtos.Profile.Phone
{
    public class CreatePhoneDto
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
    }


}
