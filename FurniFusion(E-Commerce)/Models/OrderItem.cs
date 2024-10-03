using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class OrderItem
{
    public int ItemId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
