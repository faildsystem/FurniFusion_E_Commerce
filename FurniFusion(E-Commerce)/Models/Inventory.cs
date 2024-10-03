using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public string WarehouseLocation { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<InventoryProduct> InventoryProducts { get; set; } = new List<InventoryProduct>();
}
