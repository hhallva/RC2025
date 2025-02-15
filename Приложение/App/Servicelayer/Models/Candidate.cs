namespace DataLayer.Models;

public partial class Candidate
{
    public int CandidateId { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public int PositionId { get; set; }

    public string Resume { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual Position Position { get; set; } = null!;
}
