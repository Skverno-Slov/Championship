using System;
using System.Collections.Generic;

namespace WEMMApi.Models;

public partial class MachineProduct
{
    public string MachineId { get; set; } = null!;

    public string ProductId { get; set; } = null!;

    public decimal Price { get; set; }

    public short MinStock { get; set; }

    public short QuantityAvailable { get; set; }

    public virtual VendingMachine Machine { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
