using FurniFusion.Interfaces;
using FurniFusion.Queries;
using FurniFusion.Dtos.ProductManager.Discount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FurniFusion.Mappers;

namespace FurniFusion.Controllers
{
    [Authorize(Roles = "superAdmin, productManager")]
    [ApiController]
    [Route(("api/[controller]"))]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet("getDiscounts")]
        public async Task<IActionResult> GetDiscounts([FromQuery] DiscountFilter filter)
        {
            try {

                var result = await _discountService.GetAllDiscountsAsync(filter);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                var discounts = result.Data.Select(d => d.ToDiscountDto()).ToList();
                var TotalDiscounts = result.Data.Count;
                
                return Ok(new { TotalDiscounts, discounts });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("getDiscountById/{id}")]
        public async Task<IActionResult> GetDiscountById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _discountService.GetDiscountByIdAsync(id);

                if(!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data.ToDiscountDto());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("getActiveDiscounts")]
        public async Task<IActionResult> GetActiveDiscounts()
        {
            try { 
                var result = await _discountService.GetActiveDiscountsAsync();

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                var discountsDto = result.Data.Select(d => d.ToDiscountDto()).ToList();
                var TotalDiscounts = result.Data.Count;

                return Ok(new { TotalDiscounts, discounts = discountsDto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("getDiscountByCode/{code}")]
        public async Task<IActionResult> GetDiscountByCode(string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _discountService.GetDiscountByCodeAsync(code);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                var discount = result.Data.ToDiscountDto();

                return Ok(new { discount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("validateDiscountCode/{code}")]
        public async Task<IActionResult> ValidateDiscountCode(string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _discountService.ValidateDiscountCodeAsync(code);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }    
        }

        [HttpPut("deactivateDiscount/{id}")]
        public async Task<IActionResult> DeactivateDiscount(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _discountService.DeactivateDiscountAsync(id);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("activateDiscount/{id}")]
        public async Task<IActionResult> ActivateDiscount(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _discountService.ActivateDiscountAsync(id);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("createDiscount")]
        public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountDto discountDto)
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

                var result = await _discountService.CreateDiscountAsync(discountDto, creatorId!);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return CreatedAtAction(nameof(CreateDiscount), new { id = result.Data!.DiscountId }, result.Data.ToDiscountDto());

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

        }

        [HttpPut("updateDiscount")]
        public async Task<IActionResult> UpdateDiscount([FromBody] UpdateDiscountDto discountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(updatorId))
                return Unauthorized(new { message = "User not found" });

            try
            {
                var result = await _discountService.UpdateDiscountAsync(discountDto, updatorId!);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                var discount = result.Data.ToDiscountDto();

                return Ok(new { discount });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("deleteDiscount/{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _discountService.DeleteDiscountAsync(id);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = "Discount deleted successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
