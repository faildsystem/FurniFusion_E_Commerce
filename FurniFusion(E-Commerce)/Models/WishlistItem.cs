using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class WishlistItem
{
    public int WishlistId { get; set; }

    public int ProductId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Wishlist Wishlist { get; set; } = null!;
}
