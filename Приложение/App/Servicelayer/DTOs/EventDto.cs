using System.Text;
using System.Text.Json.Serialization;

namespace DataLayer.DTOs
{
    public class EventDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("headline")]
        public string Title { get; set; }

        [JsonPropertyName("abstract")]
        public string Description { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        public string IcsData
        {
            get
            {
                StringBuilder vCard = new();
                vCard.AppendLine($"BEGIN:VCALENDAR");
                vCard.AppendLine($"VERSION:2.0");
                vCard.AppendLine($"BEGIN:VEVENT");
                vCard.AppendLine($"SUMMARY:{Title}");
                vCard.AppendLine($"DTSTART:{Date}");
                vCard.AppendLine($"UID:{Id}");
                vCard.AppendLine($"DESCRIPTION:{Description}");
                vCard.AppendLine($"ORGANIZER:{Author}");
                vCard.AppendLine($"STATUS:CONFIRMED");
                vCard.AppendLine($"PRIORITY:0");
                vCard.AppendLine($"END:VEVENT");
                vCard.AppendLine($"END:VCALENDAR");

                return vCard.ToString();
            }
        }

    }
}