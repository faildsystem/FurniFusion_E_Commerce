using FurniFusion.Dtos.SuperAdmin;
using FurniFusion.Queries;
using FurniFusion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FurniFusion.Interfaces;
using FurniFusion.Dtos.SuperAdmin.User;

namespace FurniFusion.Controllers
{
    [Authorize(Roles = "superAdmin")]
    [ApiController]
    [Route(("api/[controller]"))]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers([FromQuery] UserFilter query)
        {
            var users = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(query.email))
            {
                users = users.Where(u => u.Email!.Contains(query.email));
            }

            if (!string.IsNullOrEmpty(query.userName))
            {
                users = users.Where(u => u.UserName!.Contains(query.userName));
            }

            if (!string.IsNullOrEmpty(query.Role))
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(query.Role);
                users = users.Where(u => usersInRole.Contains(u));
            }

            var totalItems = await users.CountAsync();

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            var usersList = await users.Skip(skipNumber).Take(query.PageSize).ToListAsync();

            var usersDto = usersList.Select(user => new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Roles = _userManager.GetRolesAsync(user).Result.ToList()
            }).ToList();

            return Ok(new { totalItems, users = usersDto });

        }

        [HttpPost("deleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDto deleteUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _userManager.FindByEmailAsync(deleteUserDto.UserEmail!);
                if (user == null) return NotFound(new { message = "User not found!" });

                if (user.Email == deleteUserDto.UserEmail)
                {
                    return BadRequest(new { message = "Cannot delete yourself" });
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                // Check if the user is a superAdmin
                if (userRoles.Contains("superAdmin"))
                {
                    return BadRequest(new { message = "Cannot delete superAdmin user" });
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded) return BadRequest(new { message = "Failed to delete user" });

                return Ok(new { message = "User deleted successfully" });

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
