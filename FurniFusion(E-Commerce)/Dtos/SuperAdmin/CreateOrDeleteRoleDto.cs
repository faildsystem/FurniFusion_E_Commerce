using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin
{
    public class CreateOrDeleteRoleDto
    {
        [Required]
        public string? RoleName { get; set; }
    }
}