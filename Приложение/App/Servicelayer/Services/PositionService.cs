using DataLayer.Models;
using System.Net.Http.Json;

namespace DataLayer.Services
{
    public class PositionService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:5297/api/v1/Positions/";

        public PositionService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new(_baseUrl);
        }

        public async Task<List<Position?>> GetAsync()
            => await _client.GetFromJsonAsync<List<Position?>>("");
    }
}
