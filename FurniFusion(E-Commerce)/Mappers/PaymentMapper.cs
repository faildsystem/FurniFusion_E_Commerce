using FurniFusion.Dtos.Order;
using FurniFusion.Models;

namespace FurniFusion.Mappers
{
    public static class PaymentMapper
    {

        public static PaymentDto ToPaymentDto(this Payment payment)
        {

            return new PaymentDto
            {
                Amount = payment.Amount,

                Date = (DateTime)payment.Date!,

                PaymentMethod = payment.PaymentMethodNavigation!.MethodName,

                PaymentStatus = payment.PaymentStatus!.StatusName

            };


        }
    }
}
