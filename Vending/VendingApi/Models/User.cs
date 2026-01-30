using System;
using System.Collections.Generic;

namespace VendingApi.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? HashedPassword { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public byte RoleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<VendingMachine> VendingMachines { get; set; } = new List<VendingMachine>();

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
