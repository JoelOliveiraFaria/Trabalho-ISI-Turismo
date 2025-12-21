using TravelAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace TravelAPI.Services
{
    public class RemoteCalendarService : ICalendarService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public RemoteCalendarService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<string>> CheckConflictsAsync(DateTime start, DateTime end)
        {
            var baseUrl = _configuration["CalendarServiceUrl"];

            if (string.IsNullOrEmpty(baseUrl)) return new List<string>();

            var cleanUrl = baseUrl.TrimEnd('/');
            var url = $"{cleanUrl}/api/calendar/check";

            var payload = new { Start = start, End = end };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, payload);
                if (response.IsSuccessStatusCode)
                {
                    var conflicts = await response.Content.ReadFromJsonAsync<List<string>>();
                    return conflicts ?? new List<string>();
                }
                else
                {
                    // Isto vai ajudar-nos a ver o erro se falhar
                    Console.WriteLine($"Erro Calendário: {response.StatusCode} no URL: {url}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção Calendário: {ex.Message}");
            }
            return new List<string>();
        }
    }
}