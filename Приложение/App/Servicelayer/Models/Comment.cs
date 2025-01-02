using System;
using System.Collections.Generic;

namespace Servicelayer.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int EmployeeId { get; set; }

    public int DocumentId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Document Document { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
