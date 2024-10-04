using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.Profile.Phone
{
    public class UpdatePhoneDto
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
    }


}
