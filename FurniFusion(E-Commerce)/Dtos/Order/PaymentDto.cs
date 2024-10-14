namespace FurniFusion.Dtos.Order
{
    public class PaymentDto
    {
        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string? PaymentMethod { get; set; }

        public string? PaymentStatus { get; set; }
    }
}