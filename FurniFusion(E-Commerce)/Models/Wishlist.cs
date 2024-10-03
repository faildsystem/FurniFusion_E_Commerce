using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class Wishlist
{
    public int WishlistId { get; set; }

    public string? UserId { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
}
