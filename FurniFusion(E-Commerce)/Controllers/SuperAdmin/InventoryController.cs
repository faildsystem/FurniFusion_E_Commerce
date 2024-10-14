//using FurniFusion.Dtos.SuperAdmin.Inventory;
//using FurniFusion.Interfaces;
//using FurniFusion.Mappers;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace FurniFusion.Controllers.SuperAdmin
//{
//    [Authorize(Roles = "superAdmin, productManager")]
//    [ApiController]
//    [Route(("api/[controller]"))]
//    public class InventoryController : ControllerBase
//    {
//        private readonly IInventoryService _inventoryService;

//        public InventoryController(IInventoryService inventoryService)
//        {
//            _inventoryService = inventoryService;
//        }

//        [HttpGet("getInventories")]
//        public async Task<IActionResult> GetAllInventoriesAsync()
//        {
//            try
//            {
//                var result = await _inventoryService.GetAllInventoriesAsync();

//                if (!result.Success)
//                    return StatusCode(result.StatusCode, new { message = result.Message });


//                var inventories = result.Data.Select(c => c.ToInventoryDto()).ToList();

//                var totalItems = result.Data.Count();

//                return Ok(new { totalItems, inventories });

//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = ex.Message });
//            }
//        }

//        [HttpGet("getInventory/{id}")]
//        public async Task<IActionResult> GetInventoryByIdAsync(int id)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var result = await _inventoryService.GetInventoryByIdAsync(id);

//                if (!result.Success)
//                {
//                    return StatusCode(result.StatusCode, new { message = result.Message });
//                }

//                var inventory = result.Data.ToInventoryDto();

//                return Ok(inventory);

//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = ex.Message });
//            }
//        }

//        [HttpPost("createInventory")]
//        public async Task<IActionResult> CreateInventory([FromBody] CreateInventoryDto inventoryDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var result = await _inventoryService.CreateInventoryAsync(inventoryDto);

//                if (!result.Success)
//                {
//                    return StatusCode(result.StatusCode, new { message = result.Message });
//                }

//                return CreatedAtAction(nameof(GetInventoryByIdAsync), new { id = result.Data.InventoryId }, result.Data.ToInventoryDto());

//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = ex.Message });
//            }
//        }

//        [HttpPut("updateInventory")]
//        public async Task<IActionResult> UpdateInventory([FromBody] UpdateInventoryDto inventoryDto)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var result = await _inventoryService.UpdateInventoryAsync(inventoryDto);

//                if (!result.Success)
//                {
//                    return StatusCode(result.StatusCode, new { message = result.Message });
//                }

//                return Ok(result.Data.ToInventoryDto());

//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = ex.Message });
//            }
//        }

//        [HttpDelete("deleteInventory/{id}")]
//        public async Task<IActionResult> DeleteInventory(int id)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            try
//            {
//                var result = await _inventoryService.DeleteInventoryAsync(id);

//                if (!result.Success)
//                {
//                    return StatusCode(result.StatusCode, new { message = result.Message });
//                }

//                return Ok(new { message = result.Message });

//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, new { message = ex.Message });
//            }
//        }

//    }
//}
