using System.Text.Json.Serialization;

namespace DataLayer.DTOs
{
    public class EventsResponseDto
    {
        [JsonPropertyName("news")]
        public List<EventDto> Events { get; set; }
    }
}