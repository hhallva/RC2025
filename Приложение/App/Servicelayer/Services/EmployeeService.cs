using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Services
{
    public class EmployeeService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:5297/api/v1/Employees/";

        public EmployeeService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new(_baseUrl);
        }

        public async Task DismissAsync(int id) 
            => await _client.PatchAsJsonAsync($"{id}", id);

        public async Task UpdateAsync(Employee employee) 
            => await _client.PutAsJsonAsync($"{employee.EmployeeId}", employee);

        public async Task<Employee?> GetAsync(int id) 
            => await _client.GetFromJsonAsync<Employee>($"{id}");
    }
}
