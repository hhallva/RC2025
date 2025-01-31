using DataLayer.Models;
using System.Net.Http.Json;

namespace DataLayer.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:5297/api/v1/Employees/";

        public EmployeeService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new(_baseUrl);
            _client.GetStringAsync($"1");
        }

        public EmployeeService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new(_baseUrl);
        }

        public async Task<List<Employee?>> GetAllAsync()
           => await _client.GetFromJsonAsync<List<Employee?>>("");

        public async Task<Employee?> GetAsync(int id)
           => await _client.GetFromJsonAsync<Employee>($"{id}");

        public async Task UpdateAsync(Employee employee)
        {
            HttpResponseMessage response =
                await _client.PutAsJsonAsync($"{employee.EmployeeId}", employee);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            HttpResponseMessage response =
                await _client.PostAsJsonAsync("", employee);
            response.EnsureSuccessStatusCode();
        }

        public async Task DismissAsync(int id)
        {
            HttpResponseMessage response =
                await _client.PatchAsJsonAsync($"{id}", id);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddAbsenseEventAsync(AbsenceEvent absenceEvent)
        {
            HttpResponseMessage response =
                await _client.PostAsJsonAsync($"{absenceEvent.EmployeeId}/AbsenceEvent", absenceEvent);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddEventAsync(int id, Event education)
        {
            HttpResponseMessage response =
                await _client.PostAsJsonAsync($"{id}/Event", education);
            response.EnsureSuccessStatusCode();
        }
    }
}
