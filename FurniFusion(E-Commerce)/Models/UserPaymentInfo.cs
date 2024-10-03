using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class UserPaymentInfo
{
    public int PaymentInfoId { get; set; }

    public string? UserId { get; set; }

    public string CardNumber { get; set; } = null!;

    public string CardType { get; set; } = null!;

    public DateOnly ExpiryDate { get; set; }

    public string BillingAddress { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? User { get; set; }
}
