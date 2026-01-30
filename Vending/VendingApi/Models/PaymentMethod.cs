using System;
using System.Collections.Generic;

namespace VendingApi.Models;

public partial class PaymentMethod
{
    public byte PaymentMethodId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
