using System;
using System.Collections.Generic;

namespace Servicelayer.Models;

public partial class MaterialComment
{
    public int CommentId { get; set; }

    public int EmployeeId { get; set; }

    public int MaterialId { get; set; }

    public string Comment { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Material Material { get; set; } = null!;
}
