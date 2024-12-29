using System;
using System.Collections.Generic;

namespace Servicelayer.Models;

public partial class Event
{
    public int EventId { get; set; }

    public int EventNameId { get; set; }

    public string Type { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int? ResponsibleEmployeeId { get; set; }

    public string Status { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<DepartmentEvent> DepartmentEvents { get; set; } = new List<DepartmentEvent>();

    public virtual EventName EventName { get; set; } = null!;

    public virtual Employee? ResponsibleEmployee { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Material> Materioals { get; set; } = new List<Material>();
}
