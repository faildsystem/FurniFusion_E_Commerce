using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public List<string>? ImageUrls { get; set; }

    public string? Dimensions { get; set; }

    public decimal? Weight { get; set; }

    public List<string>? Colors { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int? StockQuantity { get; set; }

    public bool? IsAvailable { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public int? DiscountId { get; set; }

    public int? CategoryId { get; set; }

    public decimal? AverageRating { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual ICollection<InventoryProduct> InventoryProducts { get; set; } = new List<InventoryProduct>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductChangeLog> ProductChangeLogs { get; set; } = new List<ProductChangeLog>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
}
