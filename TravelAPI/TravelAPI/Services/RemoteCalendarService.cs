using TravelAPI.Interfaces;

namespace TravelAPI.Services
{
    public class RemoteCalendarService : ICalendarService
    {
        private readonly HttpClient _httpClient;

        public RemoteCalendarService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<string>> CheckConflictsAsync(DateTime start, DateTime end)
        {
            var url = "https://localhost:7083/api/calendar/check";

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
