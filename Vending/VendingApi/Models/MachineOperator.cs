using System;
using System.Collections.Generic;

namespace VendingApi.Models;

public partial class MachineOperator
{
    public string Phone { get; set; } = null!;

    public string MachineId { get; set; } = null!;

    public byte OperatorId { get; set; }

    public decimal Balance { get; set; }

    public virtual VendingMachine Machine { get; set; } = null!;

    public virtual Operator Operator { get; set; } = null!;
}
