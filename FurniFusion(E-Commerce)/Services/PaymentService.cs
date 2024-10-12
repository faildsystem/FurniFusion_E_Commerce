using FurniFusion.Data;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly FurniFusionDbContext _context;

        public PaymentService(FurniFusionDbContext context)
        {
            _context = context;
        }


        public async Task<int?> DoPaymentAsync(Payment payment)
        {
            var result = await _context.Payments.AddAsync(payment);

            await _context.SaveChangesAsync();

            return result.Entity.PaymentId;

        }

        public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
        {
           var result = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            return result;
        }
    }
}
