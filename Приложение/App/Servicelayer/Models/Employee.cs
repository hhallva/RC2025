using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string DepartmentId { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string? Phone { get; set; }

    public string WorkPhone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Cabinet { get; set; } = null!;

    public int PositionId { get; set; }

    public DateTime? Birthday { get; set; } 

    public string? Password { get; set; }

    public DateTime? DismissalDate { get; set; }

    [NotMapped]
    public virtual ICollection<AbsenceEvent> AbsenceEventEmployees { get; set; } = new List<AbsenceEvent>();

    [NotMapped]
    public virtual ICollection<AbsenceEvent> AbsenceEventReplacementEmployees { get; set; } = new List<AbsenceEvent>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [JsonIgnore , NotMapped]
    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    [NotMapped]
    public virtual ICollection<Event> EventsNavigation { get; set; } = new List<Event>();

    [NotMapped]
    public virtual Position Position { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
