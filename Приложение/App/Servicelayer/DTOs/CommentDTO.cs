using Servicelayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicelayer.DTOs
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public int MaterialId { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime ConfirmDate { get; set; }
        public AuthorCommentDTO Author { get; set; } = null!;
    }
}
