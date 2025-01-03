namespace ServiceLayer.Models;

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

    public virtual EventName EventName { get; set; } = null!;

    public virtual Employee? ResponsibleEmployee { get; set; }

    public virtual ICollection<Department> Dapartments { get; set; } = new List<Department>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
