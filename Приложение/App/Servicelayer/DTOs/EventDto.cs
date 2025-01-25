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
        public string Annotation { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
    }
}