using ServiceLayer.Models;

namespace ServiceLayer.DTOs
{
    public static class CommentMapper
    {
        public static CommentDto ToDto(this Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            return new CommentDto
            {
                Id = comment.CommentId,
                DocumentId = comment.DocumentId,
                Text = comment.Text,
                CreatedDate = comment.CreatedDate,
                UpdatedDate = comment.UpdatedDate,
                Author = new AuthorDto
                {
                    Id = comment.Employee.EmployeeId,
                    Name = $"{comment.Employee.Surname} {comment.Employee.Name}",
                    Position = comment.Employee.Position.Name
                }
            };
        }

    }
}
