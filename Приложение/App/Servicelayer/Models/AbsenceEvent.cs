using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class AbsenceEvent
{
    public int AbsenceEventId { get; set; }

    public DateTime StartDate { get; set; }

    public int DaysCount { get; set; }

    public string AbsenceType { get; set; } = null!;

    public string? Description { get; set; }

    public int? ReplasementEmployeeId { get; set; }

    public int EmployeeId { get; set; }

    [JsonIgnore]
    public virtual Employee Employee { get; set; } = null!;

    [JsonIgnore]
    public virtual Employee? ReplasementEmployee { get; set; }
}
