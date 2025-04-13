using DataLayer.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DataLayer.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:5297/api/v1/";

        public EmployeeService() : this(new HttpClient())
        {
            _client.GetStringAsync($"1");
        }

        public EmployeeService(HttpClient client)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppState.AuthToken);
        }

        public async Task<List<Employee?>> GetAllAsync()
        {
            return await _client.GetFromJsonAsync<List<Employee?>>("");
        }

        public async Task<List<Employee?>> GetAllProtectedAsync()
        {
            var response = await _client.GetAsync("Employees");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<Employee?>>();
        }

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
