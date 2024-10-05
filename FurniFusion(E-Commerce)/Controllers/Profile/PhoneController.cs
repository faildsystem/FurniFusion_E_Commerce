using FurniFusion.Models;
using FurniFusion.Dtos.Profile.Phone;
using FurniFusion.Interfaces;
using FurniFusion.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers.Profile
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
                var createdPhone = await _phoneService.AddPhoneAsync(phone, userId);

                if (createdPhone == null)
                    return BadRequest(new { message = "Failed to add phone number" });

                return Ok(createdPhone.ToPhoneDto());
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
                var phones = await _phoneService.GetAllPhoneByUserIdAsync(userId);

                var phoneDtos = phones.Select(p => p.ToPhoneDto()).ToList();

                return Ok(phoneDtos);

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
                var phone = await _phoneService.GetPhoneAsync(phoneNumber, userId);

                if (phone == null)
                    return NotFound(new { message = "Phone number not found" });

                return Ok(phone.ToPhoneDto());

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

                var success = await _phoneService.DeletePhoneAsync(phoneNumber, userId);

                if (!success)
                    return NotFound(new { message = "Phone number not found" });

                return Ok(new { message = "Phone number deleted successfully" });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }

        }

        //// Update a phone number
        //[HttpPut("{phoneNumber}")]
        //public async Task<IActionResult> UpdatePhone([FromRoute] string phoneNumber, [FromBody] UpdatePhoneDto updatePhoneDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //        return Unauthorized(new { message = "User not found" });


        //    var updatedPhone = await _phoneService.UpdatePhoneAsync(phoneNumber, userId, updatePhoneDto);

        //    if (updatedPhone == null)
        //        return NotFound(new { message = "Phone number not found" });

        //    return Ok(updatedPhone.ToPhoneDto());
        //}

    }
}
