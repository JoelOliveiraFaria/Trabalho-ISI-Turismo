using TravelAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Nodes;

namespace TravelAPI.Services
{
    public class OpenWeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenWeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetWeatherAsync(string city)
        {
            var baseUrl = _configuration["WeatherServiceUrl"];

            if (string.IsNullOrEmpty(baseUrl)) return "Configuração de meteorologia em falta.";

            var cleanUrl = baseUrl.TrimEnd('/');
            var url = $"{cleanUrl}/api/weather/{city}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Lê a resposta JSON do microsserviço
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // Como o microsserviço retorna { "text": "Sol, 20ºC" }, vamos buscar essa propriedade
                    var node = JsonNode.Parse(jsonString);
                    return node?["text"]?.ToString() ?? "Dados inválidos.";
                }
                else
                {
                    Console.WriteLine($"Erro Meteorologia: {response.StatusCode} no URL: {url}");
                    return "Informação meteorológica indisponível (Erro remoto).";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exceção Meteorologia: {ex.Message}");
                return "Erro ao contactar serviço de meteorologia.";
            }
        }
    }
}