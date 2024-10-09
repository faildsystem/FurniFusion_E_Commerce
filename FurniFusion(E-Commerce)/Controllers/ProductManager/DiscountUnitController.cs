using FurniFusion.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FurniFusion.Mappers;
using FurniFusion.Dtos.ProductManager.DiscountUnit;


namespace FurniFusion.Controllers
{
    [Authorize(Roles = "superAdmin, productManager")]
    [ApiController]
    [Route(("api/[controller]"))]
    public class DiscountUnitController : ControllerBase
    {
        private readonly IDiscountUnitService _discountUnitService;

        public DiscountUnitController(IDiscountUnitService discountUnitService)
        {
            _discountUnitService = discountUnitService;
        }

        [HttpGet("getDiscountUnits")]
        public async Task<IActionResult> GetDiscountUnits()
        {
            try
            {
                var result = await _discountUnitService.GetAllDiscountUnitsAsync();

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                var discountUnits = result.Data.Select(d => d.ToDiscountUnitDto()).ToList();

                var TotalDiscountUnits = result.Data.Count;

                return Ok(new { TotalDiscountUnits, discountUnits });
            } 
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("getDiscountUnit/{id}")]
        public async Task<IActionResult> GetDiscountUnit(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _discountUnitService.GetDiscountUnitByIdAsync(id);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data.ToDiscountUnitDto());
            } 
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("createDiscountUnit")]
        public async Task<IActionResult> CreateDiscountUnit([FromBody] CreateDiscountUnitDto discountUnitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _discountUnitService.CreateDiscountUnitAsync(discountUnitDto);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return CreatedAtAction(nameof(GetDiscountUnits), new { id = result.Data.UnitId }, result.Data.ToDiscountUnitDto());
            } 
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            
        }

        [HttpPut("updateDiscountUnit")]
        public async Task<IActionResult> UpdateDiscountUnit([FromBody] UpdateDiscountUnitDto discountUnitDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _discountUnitService.UpdateDiscountUnitAsync(discountUnitDto);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = "Discount unit updated successfully" });

            } 
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
            
        }

        [HttpDelete("deleteDiscountUnit/{id}")]
        public async Task<IActionResult> DeleteDiscountUnit(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _discountUnitService.DeleteDiscountUnitAsync(id);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = "Discount unit deleted successfully" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });

            }
        }
    }
}
