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

        // Add a phone number
        [HttpPost]
        public async Task<IActionResult> AddPhone([FromBody] CreatePhoneDto phoneDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            var phone = new UserPhoneNumber
            {
                PhoneNumber = phoneDto.PhoneNumber,
                UserId = userId,
                CreatedAt = DateTime.Now, 
                UpdatedAt = DateTime.Now
            };

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

        // Get all phone numbers by user ID
        [HttpGet]
        public async Task<IActionResult> GetPhones()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

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

        //[HttpGet("{phoneId}")]
        //public async Task<IActionResult> GetPhone([FromRoute] int phoneId)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //        return Unauthorized();

        //    try
        //    {
        //        var phone = await _phoneService.GetPhoneByIdAsync(phoneId, userId);

        //       if (phone == null)
        //            return NotFound();

        //        return Ok(phone.ToPhoneDto());

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = ex.Message });
        //    }
        //}

        // Update a phone number
        [HttpPut("{phoneId}")]
        public async Task<IActionResult> UpdatePhone(int phoneId, [FromBody] UpdatePhoneDto phoneDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });


            var updatedPhone = await _phoneService.UpdatePhoneAsync(phoneDto, userId);

            if (updatedPhone == null)
                return NotFound();

            return Ok(updatedPhone.ToPhoneDto());
        }

        // Delete a phone number
        [HttpDelete("{phoneId}")]
        public async Task<IActionResult> DeletePhone(string phoneNumber)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            var success = await _phoneService.DeletePhoneAsync(phoneNumber, userId);
            if (!success)
                return NotFound();

            return Ok(new { message = "Phone number deleted successfully" });
        }
    }
}
