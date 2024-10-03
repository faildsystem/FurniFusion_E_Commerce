using System;
using System.Collections.Generic;

namespace FurniFusion.Models;

public partial class UserPhoneNumber
{
    public string UserId { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
