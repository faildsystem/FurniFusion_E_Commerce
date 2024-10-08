using FurniFusion.Models;
using FurniFusion.Dtos.Profile.Phone;
using FurniFusion.Interfaces;
using FurniFusion.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers.user
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PhoneController : ControllerBase
    {
        private readonly IPhoneService _phoneService;


        public PhoneController(IPhoneService phoneService)
        {
            _phoneService = phoneService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPhone([FromBody] CreatePhoneDto phoneDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            var phone = phoneDto.ToUserPhoneNumber(userId);

            try
            {
                var result = await _phoneService.AddPhoneAsync(phone, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data.ToPhoneDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPhones()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            try
            {
                var result = await _phoneService.GetAllPhoneByUserIdAsync(userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data.Select(p => p.ToPhoneDto()).ToList());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{phoneNumber}")]
        public async Task<IActionResult> GetPhone([FromRoute] string phoneNumber)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            try
            {
                var result = await _phoneService.GetPhoneAsync(phoneNumber, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data?.ToPhoneDto());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Delete a phone number
        [HttpDelete("{phoneNumber}")]
        public async Task<IActionResult> DeletePhone([FromRoute] string phoneNumber)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });
            try
            {

                var result = await _phoneService.DeletePhoneAsync(phoneNumber, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }

        }

    }
}
