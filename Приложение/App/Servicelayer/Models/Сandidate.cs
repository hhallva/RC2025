namespace ServiceLayer.Models;

public partial class Сandidate
{
    public int CandidateId { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public int DesiredPosition { get; set; }

    public string Resume { get; set; } = null!;

    public DateOnly DateReceived { get; set; }

    public virtual Position DesiredPositionNavigation { get; set; } = null!;
}
