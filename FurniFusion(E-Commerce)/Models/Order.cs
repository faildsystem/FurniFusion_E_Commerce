using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string? UserId { get; set; }

    public int? Status { get; set; }

    public string AddressToDeliver { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? DiscountId { get; set; }

    public int? PaymentId { get; set; }

    public int? ShippingId { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Payment? Payment { get; set; }

    public virtual Shipping? Shipping { get; set; }

    public virtual OrderStatus? StatusNavigation { get; set; }

    public virtual User? User { get; set; }
}
