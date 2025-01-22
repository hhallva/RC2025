using DataLayer.Models;
using System.Net.Http.Json;

namespace DataLayer.Services
{
    public class WorkingCalendarService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:5297/api/v1/WorkingCalendars/";

        public WorkingCalendarService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new(_baseUrl);
        }

        public WorkingCalendarService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new(_baseUrl);
        }

        public async Task<List<WorkingCalendar?>> GetAsync()
            => await _client.GetFromJsonAsync<List<WorkingCalendar?>>("");
    }
}

