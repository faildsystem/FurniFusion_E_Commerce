using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class CategoryChangeLog
{
    public int LogId { get; set; }

    public string? OldCategoryName { get; set; }

    public string? NewCategoryName { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? ChangedAt { get; set; }

    public string? ActionType { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
