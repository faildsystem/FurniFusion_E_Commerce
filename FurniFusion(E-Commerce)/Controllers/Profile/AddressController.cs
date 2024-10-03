//using FurniFusion_E_Commerce_.Dtos.Profile.Address;
//using FurniFusion_E_Commerce_.Interfaces;
//using FurniFusion_E_Commerce_.Mappers;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace FurniFusion_E_Commerce_.Controllers.Profile
//{
//    [ApiController]
//    [Authorize]
//    [Route("api/[controller]/[action]")]
//    public class AddressController : ControllerBase
//    {

//        private readonly IAddressService _profileService;

//        public AddressController(IAddressService profileService)
//        {
//            _profileService = profileService;
//        }


//        [HttpGet("{addressId}")]
//        public async Task<IActionResult> GetAddress([FromRoute] int addressId)
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (string.IsNullOrEmpty(userId))
//            {
//                return Unauthorized(new { message = "User not found" });
//            }

//            var address = await _profileService.GetAddressAsync(addressId, userId);

//            if (address == null)
//                return NotFound(new { message = "Address not found" });

//            return Ok(address.ToAddressDto());
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllAddresses()
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (string.IsNullOrEmpty(userId))
//            {
//                return Unauthorized(new { message = "User not found" });
//            }

//            var addresses = await _profileService.GetAllAddressesAsync(userId);

//            return Ok(addresses.Select(a => a.ToAddressDto()));
//        }


//        [HttpPost]
//        public async Task<IActionResult> AddAddress([FromBody] AddAddressDto addAddressDto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            try
//            {
//                // Get the user's ID from the claims
//                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


//                if (string.IsNullOrEmpty(userId))
//                {
//                    return Unauthorized(new { message = "User not found" });
//                }

//                var address = new UserAddress
//                {
//                    UserId = userId,
//                    Country = addAddressDto.Country,
//                    City = addAddressDto.City,
//                    State = addAddressDto.State,
//                    Street = addAddressDto.Street,
//                    PostalCode = addAddressDto.PostalCode,
//                    IsPrimaryAddress = addAddressDto.IsPrimaryAddress,
//                    CreatedAt = DateTime.Now,
//                    UpdatedAt = DateTime.Now
//                };


//                var createdAddress = await _profileService.AddAddressAsync(address, userId);

//                if (createdAddress == null)
//                    return BadRequest(new { message = "Failed to add address" });


//                return CreatedAtAction(nameof(AddAddress), new { id = createdAddress.AddressId }, createdAddress.ToAddressDto());

//            }
//            catch (Exception ex)
//            {

//                return StatusCode(500, new { message = ex.Message });
//            }

//        }

//        [HttpPut]
//        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressDto updateAddressDto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);


//            try
//            {
//                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//                if (string.IsNullOrEmpty(userId))
//                {
//                    return Unauthorized(new { message = "User not found" });
//                }

//                var address = await _profileService.UpdateAddressAsync(updateAddressDto, userId);

//                if (address == null)
//                    return NotFound(new { message = "Address not found" });

//                return Ok(address.ToAddressDto());

//            }
//            catch (Exception ex)
//            {

//                return StatusCode(500, ex.Message);
//            }
//        }


//        [HttpDelete("{addressId}")]
//        public async Task<IActionResult> DeleteAddress([FromRoute] int addressId)
//        {
//            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//            if (string.IsNullOrEmpty(userId))
//            {
//                return Unauthorized(new { message = "User not found" });
//            }

//            var isDeleted = await _profileService.DeleteAddressAsync(addressId, userId);

//            if (!isDeleted)
//                return NotFound(new { message = "Address not found" });

//            return Ok(new { message = "Address deleted successfully" });
//        }

//    }
//}
