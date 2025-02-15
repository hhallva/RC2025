using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class AbsenceEvent
{
    public int AbsenceEventId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public int? ReplacementEmployeeId { get; set; }

    public int EmployeeId { get; set; }

    [JsonIgnore]
    [NotMapped]
    public virtual Employee? Employee { get; set; } = null!;

    [JsonIgnore]
    [NotMapped]
    public virtual Employee? ReplacementEmployee { get; set; }
}
