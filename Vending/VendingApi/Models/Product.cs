using System;
using System.Collections.Generic;

namespace VendingApi.Models;

public partial class Product
{
    public string ProductId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public double SalesTrend { get; set; }

    public virtual ICollection<MachineProduct> MachineProducts { get; set; } = new List<MachineProduct>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
