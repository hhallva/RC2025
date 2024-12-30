namespace Servicelayer.Dtos
{
    public class CommentDto
    {
        public int CommentId { get; set; }

        public int MaterialId { get; set; }

        public string Comment { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public DateTime ConfirmDate { get; set; }

        public AuthorCommentDto Author { get; set; } = null!;
    }
}
