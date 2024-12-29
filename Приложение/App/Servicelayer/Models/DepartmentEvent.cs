using System;
using System.Collections.Generic;

namespace Servicelayer.Models;

public partial class DepartmentEvent
{
    public string DapartmentId { get; set; } = null!;

    public int EventId { get; set; }

    public virtual Event Event { get; set; } = null!;
}
