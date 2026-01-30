using System;
using System.Collections.Generic;

namespace VendingApi.Models;

public partial class News
{
    public int NewsId { get; set; }

    public string UserId { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime EventDate { get; set; }

    public virtual User User { get; set; } = null!;
}
