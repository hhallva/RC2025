using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicelayer.Dtos
{
    public class MaterialDto
    {
        public int MaterialId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime ConfirmDate { get; set; }
        public string Category { get; set; } = null!;
        public bool HasCommnets { get; set; }
    }
}
