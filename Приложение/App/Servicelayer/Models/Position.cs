using System.Text.Json.Serialization;

namespace DataLayer.Models;

public partial class Position
{
    public int PositionId { get; set; }

    public string? Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    [JsonIgnore]
    public virtual ICollection<Сandidate> Сandidates { get; set; } = new List<Сandidate>();

    public override bool Equals(object? obj)
    {
        if (obj is Position position)
            return position.PositionId == PositionId;
        return false;
    }
}
