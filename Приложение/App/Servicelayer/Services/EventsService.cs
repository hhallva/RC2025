using DataLayer.DTOs;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DataLayer.Services
{
    public class EventsService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "https://fakenews.squirro.com/news/technology";

        public EventsService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new System.Uri(_baseUrl);
        }

        public async Task<List<EventDto>> GetAllAsync(int since = 0, int count = 10) 
            => (await _client.GetFromJsonAsync<EventsResponseDto>($"?since={since}&count={count}"))?.Events;
    }
}