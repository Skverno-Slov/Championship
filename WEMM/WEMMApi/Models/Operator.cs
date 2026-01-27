using System;
using System.Collections.Generic;

namespace WEMMApi.Models;

public partial class Operator
{
    public byte OperatorId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MachineOperator> MachineOperators { get; set; } = new List<MachineOperator>();
}
