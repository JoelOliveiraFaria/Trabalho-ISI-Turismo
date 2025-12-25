/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: WeatherController.cs
 * Descrição: Controlador do Microsserviço de Meteorologia.
 * Atua como um Gateway/Proxy que consome a API externa OpenWeatherMap
 * e devolve os dados simplificados para a TravelAPI.
 * ===================================================================================
 */

using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace WeatherService.Controllers
{
    /// <summary>
    /// Controlador responsável por fornecer dados meteorológicos.
    /// </summary>
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private const string API_KEY = "c088c3afe645b26323e98baacf7a3e1d";

        /// <summary>
        /// Construtor que injeta o cliente HTTP.
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para pedidos externos.</param>
        public WeatherController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Obtém o estado do tempo atual para uma cidade específica.
        /// </summary>
        /// <param name="city">Nome da cidade a pesquisar.</param>
        /// <returns>Objeto JSON com texto formatado (ex: "Céu limpo, 20ºC").</returns>
        // GET: api/weather/London
        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeather(string city)
        {
            // Construção do URL da API OpenWeatherMap (com idioma PT e unidades métricas)
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={API_KEY}&units=metric&lang=pt";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return NotFound();

                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonObject.Parse(jsonString);

                // Extração segura dos dados do JSON complexo da API externa
                string description = data!["weather"]?[0]?["description"]?.ToString()!;
                string temp = data["main"]?["temp"]?.ToString()!;

                // Formatação: Colocar primeira letra maiúscula
                if (!string.IsNullOrEmpty(description))
                {
                    description = char.ToUpper(description[0]) + description.Substring(1);
                }

                // Retorna um objeto JSON simples com o texto formatado para a TravelAPI consumir
                return Ok(new { Text = $"{description}, {temp}ºC" });
            }
            catch (Exception)
            {
                // Retorna 500 Internal Server Error sem mensagem de texto
                return StatusCode(500);
            }
        }
    }
}