using FurniFusion.Dtos.User.ProfileImage;
using FurniFusion.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers.user.Profile
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProfileImageController : ControllerBase
    {
        private readonly IProfileImageService _profileService;


        public ProfileImageController(IProfileImageService profileService)
        {
            _profileService = profileService;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfileImage(UpdateProfileImageDto updateProfileImageDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);


            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });


            try
            {

                var result = await _profileService.UploadProfileImageAsync(updateProfileImageDto.ProfileImage!, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return StatusCode(result.StatusCode, new { message = result.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }



        }
    }
}
