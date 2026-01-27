using System;
using System.Collections.Generic;

namespace WEMMApi.Models;

public partial class WorkDescription
{
    public byte WorkDescriptionId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
}
