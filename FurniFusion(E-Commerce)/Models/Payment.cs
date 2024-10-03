using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public decimal Amount { get; set; }

    public int? PaymentStatusId { get; set; }

    public DateTime? Date { get; set; }

    public int? PaymentMethod { get; set; }

    public string? TransactionId { get; set; }

    public string? UserId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual PaymentMethod? PaymentMethodNavigation { get; set; }

    public virtual PaymentStatus? PaymentStatus { get; set; }

    public virtual User? User { get; set; }
}
