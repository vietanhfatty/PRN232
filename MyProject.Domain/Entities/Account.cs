using System;
using System.Collections.Generic;

namespace MyProject.Domain.Entities;

public partial class Account
{
    public int AccountId { get; set; }

    public int StaffId { get; set; }

    public int RoleId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;
}
