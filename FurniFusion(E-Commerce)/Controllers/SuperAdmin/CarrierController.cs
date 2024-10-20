using FurniFusion.Dtos.SuperAdmin.Carrier;
using FurniFusion.Interfaces;
using FurniFusion.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers.SuperAdmin
{
    [Authorize(Roles = "superAdmin")]
    [ApiController]
    [Route(("api/[controller]"))]
    public class CarrierController : ControllerBase
    {

        private readonly ICarrierService _carrierService;

        public CarrierController(ICarrierService carrierService)
        {
            _carrierService = carrierService;
        }

        [HttpPost("createCarrier")]
        public async Task<IActionResult> CreateCarrier([FromBody] CreateCarrierDto carrierDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(creatorId))
                return Unauthorized(new { message = "User not found" });

            try
            {
                var result = await _carrierService.CreateCarrierAsync(carrierDto, creatorId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return CreatedAtAction(nameof(CreateCarrier), new { id = result.Data!.CarrierId }, result.Data.ToCarrierDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }

        [HttpPut("changeCarrierStatus/{id}")]
        public async Task<IActionResult> ChangeCarrierStatus(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _carrierService.ChangeCarrierStatusAsync(id);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data.ToCarrierDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("getCarriers")]
        public async Task<IActionResult> GetCarriers()
        {
            try
            {
                var result = await _carrierService.GetAllCarriersAsync();

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                var carriers = result.Data.Select(c => c.ToCarrierDto()).ToList();

                return Ok(carriers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("getCarrierById/{id}")]
        public async Task<IActionResult> GetCarrierById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _carrierService.GetCarrierByIdAsync(id);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data.ToCarrierDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("updateCarrier")]
        public async Task<IActionResult> UpdateCarrier([FromBody] UpdateCarrierDto carrierDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _carrierService.UpdateCarrierAsync(carrierDto);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data.ToCarrierDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("deleteCarrier/{id}")]
        public async Task<IActionResult> DeleteCarrier(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _carrierService.DeleteCarrierAsync(id);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("getActiveCarriers")]
        public async Task<IActionResult> GetActiveCarriers()
        {
            try
            {
                var result = await _carrierService.GetActiveCarriersAsync();

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                var carriers = result.Data.Select(c => c.ToCarrierDto()).ToList();

                return Ok(carriers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
