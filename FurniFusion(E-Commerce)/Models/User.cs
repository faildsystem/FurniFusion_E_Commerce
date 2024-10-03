using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class User: IdentityUser
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CategoryChangeLog> CategoryChangeLogs { get; set; } = new List<CategoryChangeLog>();

    public virtual ICollection<Category> CategoryCreatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<Category> CategoryUpdatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<DiscountChangeLog> DiscountChangeLogs { get; set; } = new List<DiscountChangeLog>();

    public virtual ICollection<Discount> DiscountCreatedByNavigations { get; set; } = new List<Discount>();

    public virtual ICollection<Discount> DiscountUpdatedByNavigations { get; set; } = new List<Discount>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<ProductChangeLog> ProductChangeLogs { get; set; } = new List<ProductChangeLog>();

    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    public virtual ICollection<Product> ProductUpdatedByNavigations { get; set; } = new List<Product>();

    public virtual ShoppingCart? ShoppingCart { get; set; }

    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();

    public virtual ICollection<UserPaymentInfo> UserPaymentInfos { get; set; } = new List<UserPaymentInfo>();

    public virtual ICollection<UserPhoneNumber> UserPhoneNumbers { get; set; } = new List<UserPhoneNumber>();

    public virtual Wishlist? Wishlist { get; set; }
}
