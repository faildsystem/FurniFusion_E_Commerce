using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class ShippingStatus
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Shipping> Shippings { get; set; } = new List<Shipping>();
}
