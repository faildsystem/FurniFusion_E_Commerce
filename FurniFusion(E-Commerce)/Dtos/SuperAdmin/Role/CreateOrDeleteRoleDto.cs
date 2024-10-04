using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin.Role
{
    public class CreateOrDeleteRoleDto
    {
        [Required]
        public string? RoleName { get; set; }
    }
}