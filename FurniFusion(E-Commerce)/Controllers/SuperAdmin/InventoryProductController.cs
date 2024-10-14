//using FurniFusion.Dtos.SuperAdmin.Inventory;
//using FurniFusion.Dtos.SuperAdmin.InventoryProduct;
//using FurniFusion.Interfaces;
//using FurniFusion.Mappers;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace FurniFusion.Controllers.SuperAdmin
//{
//    [Authorize(Roles = "superAdmin, productManager")]
//    [ApiController]
//    [Route(("api/[controller]"))]
//    public class InventoryProductController : ControllerBase
//    {
//        private readonly IInventoryProductService _inventoryProductService;

//        public InventoryProductController(IInventoryProductService inventoryProductService)
//        {
//            _inventoryProductService = inventoryProductService;
//        }

//        [HttpGet("getInventoryProducts/{id}")]
//        public async Task<IActionResult> GetInventoryProducts(int id)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var result = await _inventoryProductService.GetAllInventoryProductsAsync(id);

//                if (!result.Success)
//                {
//                    return StatusCode(result.StatusCode, new { message = result.Message });
//                }

//                var inventoryProducts = result.Data.Select(c => c.ToInventoryProductDto()).ToList();

//                return Ok(inventoryProducts);

//            } catch (Exception ex)
//            {
//                return StatusCode(500, new { message = ex.Message });
//            }
//        }

//        [HttpPost("createInventoryProduct")]
//        public async Task<IActionResult> CreateInventoryProduct([FromBody] CreateInventoryProductDto inventoryProductDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var result = await _inventoryProductService.CreateInventoryProductAsync(inventoryProductDto);

//                if (!result.Success)
//                {
//                    return StatusCode(result.StatusCode, new { message = result.Message });
//                }

//                return CreatedAtAction(nameof(GetInventoryProducts), new { id = result.Data.InventoryId }, result.Data.ToInventoryProductDto());

//            } catch (Exception ex)
//            {
//                return StatusCode(500, new { message = ex.Message });
//            }
//        }

//        [HttpDelete("deleteInventoryProduct")]
//        public async Task<IActionResult> DeleteInventoryProduct([FromBody] DeleteInventoryProductDto deleteInventoryProductDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var result = await _inventoryProductService.DeleteInventoryProductAsync(deleteInventoryProductDto);

//                if (!result.Success)
//                {
//                    return StatusCode(result.StatusCode, new { message = result.Message });
//                }

//                return Ok(new { message = result.Message });

//            } catch (Exception ex)
//            {
//                return StatusCode(500, new { message = ex.Message });
//            }
//        }

//        [HttpPut("updateInventoryProduct")]
//        public async Task<IActionResult> UpdateInventoryProduct([FromBody] UpdateInventoryProductDto updateInventoryProductDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var result = await _inventoryProductService.UpdateInventoryProductAsync(updateInventoryProductDto);

//                if (!result.Success)
//                {
//                    return StatusCode(result.StatusCode, new { message = result.Message });
//                }

//                return Ok(result.Data.ToInventoryProductDto());

//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = ex.Message });
//            }
//        }
//    }
//}
