using System;
using System.Collections.Generic;

namespace VendingApi.Models;

public partial class Sale
{
    public int SaleId { get; set; }

    public string ProductId { get; set; } = null!;

    public string MachineId { get; set; } = null!;

    public byte PaymentMethodId { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual VendingMachine Machine { get; set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
