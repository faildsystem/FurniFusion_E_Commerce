using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class ShoppingCart
{
    public int CartId { get; set; }

    public string? UserId { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();

    public virtual User? User { get; set; }
}
