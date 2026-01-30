using System;
using System.Collections.Generic;

namespace VendingApi.Models;

public partial class Template
{
    public byte TemplateId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<VendingMachine> VendingMachineCriticalThresholdTemplates { get; set; } = new List<VendingMachine>();

    public virtual ICollection<VendingMachine> VendingMachineNotificationTemplates { get; set; } = new List<VendingMachine>();
}
