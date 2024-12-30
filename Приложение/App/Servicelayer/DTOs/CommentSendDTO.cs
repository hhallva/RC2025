using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicelayer.Dtos
{
    public class CommentSendDTO
    {
       
        public int EmployeeId { get; set; }
        public string Comment { get; set; } = null!;

    }
}
