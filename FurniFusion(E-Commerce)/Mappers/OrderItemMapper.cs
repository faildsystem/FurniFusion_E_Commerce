using FurniFusion.Dtos.Order;
using FurniFusion.Models;

namespace FurniFusion.Mappers
{
    public static class OrderItemMapper
    {

        public static OrderItemDto ToOrderItemDto(this OrderItem orderItem)
        {

            return new OrderItemDto
            {

                ProductName = orderItem.Product.ProductName,
                Quantity = orderItem.Quantity,
                Price = orderItem.Product.Price
            };
        }

    }
}
