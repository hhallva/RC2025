namespace DataLayer.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }

        public int DocumentId { get; set; }

        public string Text { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        public AuthorDto Author { get; set; } = null!;
    }
}
