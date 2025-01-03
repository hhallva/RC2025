using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DTOs
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string Category { get; set; } = null!;
        public bool HasComments { get; set; }
    }
}
