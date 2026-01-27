using System;
using System.Collections.Generic;

namespace WEMMApi.Models;

public partial class Status
{
    public byte StatusId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<VendingMachine> VendingMachines { get; set; } = new List<VendingMachine>();
}
