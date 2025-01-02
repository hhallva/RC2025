using Servicelayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicelayer.Dtos
{
    public static class CommentMapper
    {
        public static CommentDto ToDto(this MaterialComment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            return new CommentDto
            {
                Id = comment.CommentId,
                DocumentId = comment.MaterialId,
                Text = comment.Comment,
                CreateDate = comment.Material.CreateDate,
                ConfirmDate = comment.Material.ConfirmDate,
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
