using System;
using System.Collections.Generic;

namespace Servicelayer.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime ConfirmDate { get; set; }

    public string Status { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? Author { get; set; }

    public virtual ICollection<MaterialComment> MaterialComments { get; set; } = new List<MaterialComment>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
