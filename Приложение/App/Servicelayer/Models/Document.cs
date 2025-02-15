namespace DataLayer.Models;

public partial class Document
{
    public int DocumentId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime ApprovedDate { get; set; }

    public string Status { get; set; } = null!;

    public string Category { get; set; } = null!;

    public int AuthorId { get; set; }

    public virtual Employee Author { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
