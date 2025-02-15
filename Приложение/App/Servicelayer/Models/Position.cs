using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class Position
{
    public int PositionId { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [JsonIgnore]
    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();

    public override bool Equals(object? obj) => (obj is Position position) ? position.PositionId == PositionId : false;
}
