using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class DiscountUnit
{
    public int UnitId { get; set; }

    public string UnitName { get; set; } = null!;

    public virtual ICollection<DiscountChangeLog> DiscountChangeLogNewDiscountUnits { get; set; } = new List<DiscountChangeLog>();

    public virtual ICollection<DiscountChangeLog> DiscountChangeLogOldDiscountUnits { get; set; } = new List<DiscountChangeLog>();

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
}
