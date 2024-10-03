using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class ShoppingCartItem
{
    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ShoppingCart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
