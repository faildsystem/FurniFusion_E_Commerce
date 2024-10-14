using FurniFusion.Models;

namespace FurniFusion.Dtos.Order
{
    public class OrderDto
    {
        public string? Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ShippingDto? ShippingDetails { get; set; }
        public PaymentDto? PaymentDetails { get; set; }
        public List<OrderItemDto>? Items { get; set; }
    }
}
