using System.Text.Json.Nodes;
using TravelAPI.Interfaces;

namespace TravelAPI.Services
{
    public class OpenWeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public OpenWeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetWeatherAsync(string city)
        {
            string url = $"http://localhost:5002/api/weather/{city}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return "Informação meteorológica indisponível";
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                var node = JsonNode.Parse(jsonString);

                return node?["text"]?.ToString() ?? "Dados inválidos";
            }
            catch (Exception)
            {
                return "Erro ao contactar serviço de meteorologia";
            }
        }
    }
}