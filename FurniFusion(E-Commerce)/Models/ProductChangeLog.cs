using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class ProductChangeLog
{
    public int LogId { get; set; }

    public int ProductId { get; set; }

    public string? OldProductName { get; set; }

    public string? NewProductName { get; set; }

    public List<string>? OldImageUrls { get; set; }

    public List<string>? NewImageUrls { get; set; }

    public string? OldDimensions { get; set; }

    public string? NewDimensions { get; set; }

    public decimal? OldWeight { get; set; }

    public decimal? NewWeight { get; set; }

    public List<string>? OldColors { get; set; }

    public List<string>? NewColors { get; set; }

    public string? OldDescription { get; set; }

    public string? NewDescription { get; set; }

    public decimal? OldPrice { get; set; }

    public decimal? NewPrice { get; set; }

    public int? OldStockQuantity { get; set; }

    public int? NewStockQuantity { get; set; }

    public bool? OldIsAvailable { get; set; }

    public bool? NewIsAvailable { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? ChangedAt { get; set; }

    public string? ActionType { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
