using FurniFusion.Models;
using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.Order
{
    public class CreateOrderDto
    {
        [Required]
        public string AddressToDeliver { get; set; } = null!;

        [Required]
        public int? PaymentMethod { get; set; }


    }
}
