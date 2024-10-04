using FurniFusion.Dtos.SuperAdmin.Role;
using Microsoft.AspNetCore.Identity;

namespace FurniFusion.Mappers
{
    public static class RoleMapper
    {
        public static RoleDto ToRoleDto(this IdentityRole role)
        {
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName
            };
        }
    }
}
