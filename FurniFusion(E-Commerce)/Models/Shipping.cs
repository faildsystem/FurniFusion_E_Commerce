using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class Shipping
{
    public int ShippingId { get; set; }

    public string ShippingMethod { get; set; } = null!;

    public decimal ShippingCost { get; set; }

    public DateOnly ShippingDate { get; set; }

    public DateOnly EstimatedDeliveryDate { get; set; }

    public int? ShippingStatusId { get; set; }

    public int? CarrierId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Carrier? Carrier { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ShippingStatus? ShippingStatus { get; set; }
}
