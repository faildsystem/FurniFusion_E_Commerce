using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class Carrier
{
    public int CarrierId { get; set; }

    public string CarrierName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Website { get; set; }

    public string Address { get; set; } = null!;

    public string? ShippingApi { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();
}
