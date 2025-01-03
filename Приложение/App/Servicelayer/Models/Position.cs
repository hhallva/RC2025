namespace ServiceLayer.Models;

public partial class Position
{
    public int PositionId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Сandidate> Сandidates { get; set; } = new List<Сandidate>();
}
