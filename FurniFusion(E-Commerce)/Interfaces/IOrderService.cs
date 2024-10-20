using FurniFusion.Dtos.Order;
using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult<bool>> CreateOrderAsync(CreateOrderDto createOrderDto, string userId);
        Task<ServiceResult<bool>> CancelOrderAsync(int orderId, string userId);

        //Task<ServiceResult<bool>> DeleteOrderAsync(int orderId, string userId);
        Task<ServiceResult<UpdateOrdersStatusDto?>> ChangeOrderStatusAsync( Dictionary<int, int> data);
        Task<ServiceResult<List<Order>?>> GetOrdersByUserIdAsync(string userId);
        Task<ServiceResult<Order?>> GetOrderByIdAsync(int orderId, string userId);
        Task<ServiceResult<Order>> ApplyDiscountToOrderAsync(int orderId, int discountId);

    }
}
