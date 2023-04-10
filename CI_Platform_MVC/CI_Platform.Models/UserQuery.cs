using System;
using System.Collections.Generic;

namespace CI_Platform.Models;

public partial class UserQuery
{
    public long QueryId { get; set; }

    public long UserId { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public virtual User User { get; set; } = null!;
}
