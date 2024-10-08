﻿using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.SuperAdmin.Role
{
    public class ChangeUserRoleDto
    {
        [Required]
        [EmailAddress]
        public string? UserEmail { get; set; }

        [Required]
        public string? RoleName { get; set; }
    }
}