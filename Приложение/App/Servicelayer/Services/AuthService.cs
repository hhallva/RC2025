using DataLayer.DTOs;
using DataLayer.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace DataLayer.Services
{
    public class AuthService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://localhost:5297/api/v1/";

        public AuthService() : this(new HttpClient())
        {
            _client.GetStringAsync($"1");
        }

        public AuthService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new(_baseUrl);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppState.AuthToken);
        }

        public async Task<string?> GetTokenAsync(LoginDto login)
        {
            var response = await _client.PostAsJsonAsync("SignIn", login);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
