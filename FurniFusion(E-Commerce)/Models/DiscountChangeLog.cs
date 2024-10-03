using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class DiscountChangeLog
{
    public int LogId { get; set; }

    public int DiscountId { get; set; }

    public string? OldDiscountCode { get; set; }

    public string? NewDiscountCode { get; set; }

    public decimal? OldDiscountValue { get; set; }

    public decimal? NewDiscountValue { get; set; }

    public int? OldDiscountUnitId { get; set; }

    public int? NewDiscountUnitId { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? ChangedAt { get; set; }

    public string? ActionType { get; set; }

    public virtual Discount Discount { get; set; } = null!;

    public virtual DiscountUnit? NewDiscountUnit { get; set; }

    public virtual DiscountUnit? OldDiscountUnit { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
