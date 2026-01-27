using System;
using System.Collections.Generic;

namespace WEMMApi.Models;

public partial class Worker
{
    public int WorkerId { get; set; }

    public string UserId { get; set; } = null!;

    public bool IsManager { get; set; }

    public bool IsEngineer { get; set; }

    public bool IsTechnician { get; set; }

    public virtual User User { get; set; } = null!;
}
