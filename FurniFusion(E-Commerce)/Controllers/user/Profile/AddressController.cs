using FurniFusion.Dtos.Profile.Address;
using FurniFusion.Interfaces;
using FurniFusion.Mappers;
using FurniFusion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers.user.Profile
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class AddressController : ControllerBase
    {

        private readonly IAddressService _profileService;

        public AddressController(IAddressService profileService)
        {
            _profileService = profileService;
        }


        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddress([FromRoute] int addressId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not found" });
            }
            try
            {

                var result = await _profileService.GetAddressAsync(addressId, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data!.ToAddressDto());
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not found" });
            }

            try
            {

                var result = await _profileService.GetAllAddressesAsync(userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data.Select(a => a.ToAddressDto()));
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] CreateAddressDto addAddressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get the user's ID from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not found" });
            }

            try
            {

                var address = addAddressDto.ToUserAddress(userId);


                var result = await _profileService.AddAddressAsync(address, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return CreatedAtAction(nameof(AddAddress), new { id = result.Data!.AddressId }, result.Data.ToAddressDto());

            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpPut("{addressId}")]
        public async Task<IActionResult> UpdateAddress([FromRoute] int addressId, [FromBody] UpdateAddressDto updateAddressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not found" });
            }

            try
            {

                var result = await _profileService.UpdateAddressAsync(updateAddressDto, addressId, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data!.ToAddressDto());

            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress([FromRoute] int addressId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User not found" });
            }

            try
            {

                var result = await _profileService.DeleteAddressAsync(addressId, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = "Address deleted successfully" });

            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
