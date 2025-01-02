namespace Servicelayer.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        public int DocumentId { get; set; }

        public string Text { get; set; } = null!;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime? ConfirmDate { get; set; }

        public AuthorDto Author { get; set; } = null!;
    }
}
