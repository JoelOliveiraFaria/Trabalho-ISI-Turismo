using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private const string API_KEY = "c088c3afe645b26323e98baacf7a3e1d";

        public WeatherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={API_KEY}&units=metric&lang=pt";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return NotFound("Cidade não encontrada ou erro na API externa.");

                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonObject.Parse(jsonString);

                string description = data!["weather"]?[0]?["description"]?.ToString()!;
                string temp = data["main"]?["temp"]?.ToString()!;

                if (!string.IsNullOrEmpty(description))
                {
                    description = char.ToUpper(description[0]) + description.Substring(1);
                }

                // Retorna um objeto JSON simples com o texto formatado
                return Ok(new { Text = $"{description}, {temp}ºC" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
