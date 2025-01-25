using DataLayer.DTOs;
using System.Net.Http.Json;

namespace DataLayer.Services
{
    public class EventsService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://fakenews.squirro.com/news/technology";

        public EventsService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new(_baseUrl);
        }

        public async Task<List<EventDto>> GetAllAsync(int since = 0, int count = 10)
            => (await _client.GetFromJsonAsync<EventsResponseDto>($"?since={since}&count={count}"))?.Events;
    }
}