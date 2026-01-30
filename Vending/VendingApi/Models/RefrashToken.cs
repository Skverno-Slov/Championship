using System;
using System.Collections.Generic;

namespace VendingApi.Models;

public partial class RefrashToken
{
    public int TokenId { get; set; }

    public string StringToken { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
