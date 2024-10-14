namespace FurniFusion.Dtos.Order
{
    public class ShippingDto
    {

        public string? ShippingMethod { get; set; }

        public decimal ShippingCost { get; set; }

        public DateOnly ShippingDate  { get; set; }

        public string? ShippingStatus { get; set; }
    }
}