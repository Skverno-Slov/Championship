using System;
using System.Collections.Generic;

namespace VendingApi.Models;

public partial class MachinePlace
{
    public byte PlaceId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<VendingMachine> VendingMachines { get; set; } = new List<VendingMachine>();
}
