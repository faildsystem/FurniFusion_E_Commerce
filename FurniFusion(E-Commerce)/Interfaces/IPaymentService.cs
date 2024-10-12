using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface IPaymentService
    {

        Task<int?> DoPaymentAsync(Payment payment);

        Task<Payment> GetPaymentByIdAsync(int paymentId);
    }
}
