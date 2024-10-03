//using FurniFusion.Dtos.SuperAdmin;
//using FurniFusion.Queries;
//using FurniFusion.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace FurniFusion.Controllers
//{
//    [Authorize(Roles = "superAdmin")]
//    [ApiController]
//    [Route(("api/[controller]"))]
//    public class SuperAdminController : ControllerBase
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly ITokenService _tokenService;

//        public SuperAdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _tokenService = tokenService;
//        }

//        [HttpGet("getUsers")]
//        public async Task<IActionResult> GetUsers([FromQuery] UserFilter query)
//        {
//            var users = _userManager.Users.AsQueryable();

//            if (!string.IsNullOrEmpty(query.email))
//            {
//                users = users.Where(u => u.Email!.Contains(query.email));
//            }

//            if (!string.IsNullOrEmpty(query.userName))
//            {
//                users = users.Where(u => u.UserName!.Contains(query.userName));
//            }

//            if (!string.IsNullOrEmpty(query.Role))
//            {
//                var usersInRole = await _userManager.GetUsersInRoleAsync(query.Role);
//                users = users.Where(u => usersInRole.Contains(u));   
//            }

//            var totalItems = await users.CountAsync();

//            var skipNumber = (query.PageNumber - 1) * query.PageSize;

//            var usersList = await users.Skip(skipNumber).Take(query.PageSize).ToListAsync();

//            var usersDto = usersList.Select(user => new UserDto
//            {
//                Id = user.Id,
//                Email = user.Email,
//                UserName = user.UserName,
//                FirstName = user.FirstName,
//                LastName = user.LastName,
//                PhoneNumber = user.PhoneNumber,
//                Roles = _userManager.GetRolesAsync(user).Result.ToList()
//            }).ToList();

//            return Ok(new {totalItems, users = usersDto});

//        }

//        [HttpGet("getRoles")]
//        public async Task<IActionResult> GetRoles()
//        {
//            var roles = await _roleManager.Roles.ToListAsync();
//            var totalItems = roles.Count;
//            return Ok(new { totalItems, roles });
//        }

//        [HttpPost("createRole")]
//        public async Task<IActionResult> CreateRole([FromBody] CreateOrDeleteRoleDto createRoleDto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            try
//            {
//                var role = new IdentityRole
//                {
//                    Name = createRoleDto.RoleName,
//                    NormalizedName = createRoleDto.RoleName!.ToUpper()
//                };

//                if (await _roleManager.RoleExistsAsync(createRoleDto.RoleName!))
//                {
//                    return BadRequest(new { message = "Role already exists" });
//                }

//                var result = await _roleManager.CreateAsync(role);
//                if (!result.Succeeded) return BadRequest(new { message = "Failed to create role" });

//                return Ok(new { message = "Role created successfully" });

//            } catch (Exception e)
//            {
//                return BadRequest(e.Message);
//            }
//        }

//        [HttpPost("deleteRole")]
//        public async Task<IActionResult> DeleteRole([FromBody] CreateOrDeleteRoleDto deleteRoleDto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            try
//            {
//                var role = await _roleManager.FindByNameAsync(deleteRoleDto.RoleName!);
//                if (role == null) return NotFound(new { message = "Role not found" });

//                if (role.Name == "superAdmin")
//                {
//                    return BadRequest(new { message = "Cannot delete superAdmin role" });
//                }

//                var result = await _roleManager.DeleteAsync(role);
//                if (!result.Succeeded) return BadRequest(new { message = "Failed to delete role" });

//                return Ok(new { message = "Role deleted successfully" });

//            } catch (Exception e)
//            {
//                return BadRequest(e.Message);
//            }
//        }

//        [HttpPost("assignRoleTo")]
//        public async Task<IActionResult> AssignRoleTo([FromBody] ChangeUserRoleDto changeUserRoleDto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            try
//            {
//                var req = new ChangeUserRoleDto
//                {
//                    UserEmail = changeUserRoleDto.UserEmail,
//                    RoleName = changeUserRoleDto.RoleName
//                };

//                var user = await _userManager.FindByEmailAsync(changeUserRoleDto.UserEmail!);

//                if (user == null) return NotFound(new { message = "User not found!" });

//                var userRoles = await _userManager.GetRolesAsync(user);

//                // Check if the user already has the role
//                if (userRoles.Contains(changeUserRoleDto.RoleName!))
//                {
//                    return BadRequest(new { message = "User already has this role." });
//                }

//                var result = await _userManager.AddToRoleAsync(user, changeUserRoleDto.RoleName!);
//                if (!result.Succeeded) return BadRequest(new { message = "Failed to add role" });

//                return Ok(new { message = "Role updated successfully" });

//            } catch (Exception e)
//            {
//                return BadRequest(e.Message);
//            }
//        }

//        [HttpPost("removeRoleFrom")]
//        public async Task<IActionResult> RemoveRoleFrom([FromBody] ChangeUserRoleDto changeUserRoleDto)
//        {

//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            try
//            {
//                var user = await _userManager.FindByEmailAsync(changeUserRoleDto.UserEmail!);
//                if (user == null) return NotFound(new { message = "Invalid Request!" });

//                var userRoles = await _userManager.GetRolesAsync(user);
                   
//                if (userRoles.Count == 1)
//                {
//                    return BadRequest(new { message = "User must have at least one role" });
//                }

//                if (userRoles.Contains("superAdmin"))
//                {
//                    return BadRequest(new { message = "Cannot remove superAdmin role" });
//                }

//                // Check if the user already has the role
//                if (!userRoles.Contains(changeUserRoleDto.RoleName!))
//                {
//                    return BadRequest(new { message = "User doesn't have this role." });
//                }

//                var result = await _userManager.RemoveFromRoleAsync(user, changeUserRoleDto.RoleName!);
//                if (!result.Succeeded) return BadRequest(new { message = "Failed to remove role" });

//                return Ok(new { message = "Role removed successfully" });

//            } catch (Exception e)
//            {
//                return BadRequest(e.Message);
//            }

            
//        }

//        [HttpPost("deleteUser")]
//        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserDto deleteUserDto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            try
//            {
//                var user = await _userManager.FindByEmailAsync(deleteUserDto.UserEmail!);
//                if (user == null) return NotFound(new { message = "User not found!" });

//                var userRoles = await _userManager.GetRolesAsync(user);

//                // Check if the user is a superAdmin
//                if (userRoles.Contains("superAdmin"))
//                {
//                    return BadRequest(new { message = "Cannot delete superAdmin user" });
//                }

//                var result = await _userManager.DeleteAsync(user);
//                if (!result.Succeeded) return BadRequest(new { message = "Failed to delete user" });

//                return Ok(new { message = "User deleted successfully" });

//            } catch (Exception e)
//            {
//                return BadRequest(e.Message);
//            }
//        }
//    }
//}
