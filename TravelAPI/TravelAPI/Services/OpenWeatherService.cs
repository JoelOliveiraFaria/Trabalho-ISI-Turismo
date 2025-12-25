/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: OpenWeatherService.cs
 * Descrição: Implementação do serviço de meteorologia (IWeatherService).
 * Responsável por consumir uma API REST externa para obter dados do tempo.
 * ===================================================================================
 */

using TravelAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Nodes;

namespace TravelAPI.Services
{
    /// <summary>
    /// Serviço que comunica com uma API Externa (Microserviço de Meteorologia) via HTTP/REST.
    /// </summary>
    public class OpenWeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor que injeta o cliente HTTP e as configurações.
        /// </summary>
        /// <param name="httpClient">Instância de HttpClient gerida pela framework.</param>
        /// <param name="configuration">Acesso ao appsettings.json para ler o URL da API.</param>
        public OpenWeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        /// <summary>
        /// Obtém a descrição do tempo para uma cidade específica.
        /// Faz um pedido GET ao endpoint da API externa e processa o JSON de resposta.
        /// </summary>
        /// <param name="city">Nome da cidade.</param>
        /// <returns>String com o estado do tempo (ex: "Sol, 25ºC") ou string vazia em caso de erro.</returns>
        public async Task<string> GetWeatherAsync(string city)
        {
            // Ler o URL base do ficheiro de configurações
            var baseUrl = _configuration["WeatherServiceUrl"];

            if (string.IsNullOrEmpty(baseUrl)) return string.Empty;

            // Construir o URL final (limpando barras duplicadas)
            var cleanUrl = baseUrl.TrimEnd('/');
            var url = $"{cleanUrl}/api/weather/{city}";

            try
            {
                // Executa o pedido HTTP GET
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Lê a resposta JSON do microsserviço
                    var jsonString = await response.Content.ReadAsStringAsync();

                    // O microsserviço retorna algo como: { "text": "Sol, 20ºC", ... }
                    // Usamos JsonNode para extrair apenas o campo "text"
                    var node = JsonNode.Parse(jsonString);
                    return node?["text"]?.ToString() ?? string.Empty;
                }
                else
                {
                    // Log de erro para debug na consola (opcional)
                    Console.WriteLine($"Erro Meteorologia: {response.StatusCode}");
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                // Em caso de falha na rede ou exceção, retorna vazio
                return string.Empty;
            }
        }
    }
}