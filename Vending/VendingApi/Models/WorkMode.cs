using System;
using System.Collections.Generic;

namespace VendingApi.Models;

public partial class WorkMode
{
    public byte WorkModeId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<VendingMachine> VendingMachines { get; set; } = new List<VendingMachine>();
}
