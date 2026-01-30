using System;
using System.Collections.Generic;

namespace WEMMApi.Models;

public partial class Maintenance
{
    public int MaintenanceId { get; set; }

    public string MachineId { get; set; } = null!;

    public DateTime Date { get; set; }

    public byte WorkDescriptionId { get; set; }

    public string? IssuesFound { get; set; }

    public string WorkerId { get; set; } = null!;

    public virtual VendingMachine Machine { get; set; } = null!;

    public virtual WorkDescription WorkDescription { get; set; } = null!;
}
