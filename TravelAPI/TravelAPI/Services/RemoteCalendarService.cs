using TravelAPI.Interfaces;

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
            var url = _configuration["CalendarServiceUrl"];

            if(string.IsNullOrEmpty(url))
            {
                Console.WriteLine("CalendarServiceUrl não está configurado.");
                return new List<string>();
            }

            var urlEndpoint = $"{url}/api/calendar/check";

            var payload = new
            {
                Start = start,
                End = end
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, payload);

                if (response.IsSuccessStatusCode)
                {
                    var conflicts = await response.Content.ReadFromJsonAsync<List<string>>();
                    return conflicts ?? new List<string>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<string>();
        }
    }
}
