using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Dtos
{
    public class MaterialDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime ConfirmDate { get; set; }
        public string Category { get; set; } = null!;
        public bool HasCommnet { get; set; }
    }
}
