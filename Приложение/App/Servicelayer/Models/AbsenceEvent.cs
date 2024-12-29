using System;
using System.Collections.Generic;

namespace Servicelayer.Models;

public partial class AbsenceEvent
{
    public int AbsenceEventId { get; set; }

    public DateOnly StartDate { get; set; }

    public int DaysCount { get; set; }

    public string AbsenceType { get; set; } = null!;

    public string? Description { get; set; }

    public int? ReplasementEmployeeId { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Employee? ReplasementEmployee { get; set; }
}
