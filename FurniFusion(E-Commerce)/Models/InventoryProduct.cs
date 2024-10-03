using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class InventoryProduct
{
    public int InventoryId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Inventory Inventory { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
