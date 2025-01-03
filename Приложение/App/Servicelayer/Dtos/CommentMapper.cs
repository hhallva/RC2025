﻿using ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos
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
