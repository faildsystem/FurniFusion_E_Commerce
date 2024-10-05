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
        
        // Discount
        [HttpGet("getDiscounts")]
        public async Task<IActionResult> GetDiscounts([FromQuery] DiscountFilter filter)
        {
            var discounts = await _discountService.GetAllDiscountsAsync(filter);
            var discountsDto = discounts.Select(d => d.ToDiscountDto()).ToList();
            var TotalDiscounts = discounts.Count;


            return Ok(new { TotalDiscounts, discountsDto });
        }

        [HttpGet("getDiscount/{id}")]
        public async Task<IActionResult> GetDiscount(int id)
        {
            try
            {
                var discount = await _discountService.GetDiscountByIdAsync(id);

                return Ok(discount.ToDiscountDto());
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("createDiscount")]
        public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountDto discountDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var discount = await _discountService.CreateDiscountAsync(discountDto, creatorId!);

                return Ok(discount.ToDiscountDto());

            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }

        }

        [HttpPut("updateDiscount")]
        public async Task<IActionResult> UpdateDiscount([FromBody] UpdateDiscountDto discountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var discount = await _discountService.UpdateDiscountAsync(discountDto, updatorId!);

                return Ok(discount.ToDiscountDto());

            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("deleteDiscount/{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            try
            {
                await _discountService.DeleteDiscountAsync(id);
                return Ok(new { message = "Discount deleted" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        // Discount Unit
        [HttpGet("getDiscountUnits")]
        public async Task<IActionResult> GetDiscountUnits()
        {
            var discountUnits = await _discountService.GetAllDiscountUnitsAsync();

            var discountUnitsDto = discountUnits.Select(d => d.ToDiscountUnitDto()).ToList();

            var TotalDiscountUnits = discountUnits.Count;

            return Ok(new { TotalDiscountUnits, discountUnitsDto });
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
                var discountUnit = await _discountService.CreateDiscountUnitAsync(discountUnitDto);

                return Ok(discountUnit.ToDiscountUnitDto());

            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
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
                var discountUnit = await _discountService.UpdateDiscountUnitAsync(discountUnitDto);

                return Ok(discountUnit.ToDiscountUnitDto());

            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpDelete("deleteDiscountUnit/{id}")]
        public async Task<IActionResult> DeleteDiscountUnit(int id)
        {
            try
            {
                await _discountService.DeleteDiscountUnitAsync(id);
                return Ok(new { message = "Discount unit deleted" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

    }
}
