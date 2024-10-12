using FurniFusion.Dtos.Order;
using FurniFusion.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FurniFusion.Controllers.user
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Make an order
        [HttpPost]
        public async Task<IActionResult> MakeOrder([FromBody] CreateOrderDto orderDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            try
            {
                var result = await _orderService.CreateOrderAsync(orderDto, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return StatusCode(result.StatusCode, new { message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Cancel an order
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            try
            {
                var result = await _orderService.CancelOrderAsync(orderId, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return StatusCode(result.StatusCode, new { message = result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Get order by ID
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder([FromRoute] int orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            try
            {
                var result = await _orderService.GetOrderByIdAsync(orderId, userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Get orders by user
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            try
            {
                var result = await _orderService.GetOrdersByUserIdAsync(userId);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Change order status (for admin or system use)
        [HttpPut]
        [Authorize(Roles = "superAdmin, productManager")]
        public async Task<IActionResult> ChangeOrdersStatus([FromBody] ChangeOrdersStatusDto changeOrdersStatusDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _orderService.ChangeOrderStatusAsync(changeOrdersStatusDto.orderStatusData);

                if (!result.Success)
                    return StatusCode(result.StatusCode, new { message = result.Message });

                return Ok(new { message = result.Message, result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
