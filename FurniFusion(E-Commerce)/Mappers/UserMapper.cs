using FurniFusion.Dtos.SuperAdmin.User;
using FurniFusion.Models;
using Microsoft.AspNetCore.Identity;

namespace FurniFusion.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User user, UserManager<User> userManager)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Roles = userManager.GetRolesAsync(user).Result.ToList()
            };
        }
    }
}
