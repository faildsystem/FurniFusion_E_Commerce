using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.Profile.Phone
{
    public class CreatePhoneDto
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
    }


}
