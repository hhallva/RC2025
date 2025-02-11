using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class NewsDto
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
