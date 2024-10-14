using FurniFusion.Dtos.Order;
using FurniFusion.Models;

namespace FurniFusion.Mappers
{
    public static class OrderMapper
    {

        public static OrderDto ToOrderDto(this Order order)
        {
            return new OrderDto
            {
                Status = order.StatusNavigation!.StatusName,
                TotalPrice = order.TotalPrice,
                CreatedAt = (DateTime)order.CreatedAt!,
                UpdatedAt = (DateTime)order.UpdatedAt!,
                ShippingDetails = order.Shipping!.ToShippingDto(),
                PaymentDetails = order.Payment!.ToPaymentDto(),
                Items = order.OrderItems.Select(o => o.ToOrderItemDto()).ToList()

            };
        }
    }
}
