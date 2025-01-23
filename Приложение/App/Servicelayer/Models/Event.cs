using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class Event
{
    public int EventId { get; set; }

    public int EventTypeId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int? ResponsibleEmployeeId { get; set; }

    public string Status { get; set; } = null!;

    public string? Description { get; set; }

    [NotMapped]
    public virtual EventType? EventType { get; set; } = null!;

    public virtual Employee? ResponsibleEmployee { get; set; }

    public virtual ICollection<Department> Dapartments { get; set; } = new List<Department>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    [JsonIgnore]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
