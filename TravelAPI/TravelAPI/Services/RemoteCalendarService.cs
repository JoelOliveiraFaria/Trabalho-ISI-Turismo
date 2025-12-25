/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: RemoteCalendarService.cs
 * Descrição: Implementação do serviço de calendário (ICalendarService).
 * Responsável por consumir uma API REST externa para verificar conflitos de agenda.
 * ===================================================================================
 */

using TravelAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace TravelAPI.Services
{
    /// <summary>
    /// Serviço que comunica com uma API Externa de Calendário via HTTP/REST.
    /// Utiliza POST para enviar datas e receber lista de conflitos.
    /// </summary>
    public class RemoteCalendarService : ICalendarService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor que injeta o cliente HTTP e as configurações.
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para fazer pedidos à API externa.</param>
        /// <param name="configuration">Acesso ao ficheiro de configurações.</param>
        public RemoteCalendarService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        /// <summary>
        /// Verifica conflitos de agenda enviando um intervalo de datas para a API externa.
        /// </summary>
        /// <param name="start">Data de início.</param>
        /// <param name="end">Data de fim.</param>
        /// <returns>Lista de strings com a descrição dos conflitos, ou lista vazia se não houver.</returns>
        public async Task<List<string>> CheckConflictsAsync(DateTime start, DateTime end)
        {
            var baseUrl = _configuration["CalendarServiceUrl"];

            // Se não houver configuração, assume que não há conflitos (fail-safe)
            if (string.IsNullOrEmpty(baseUrl)) return new List<string>();

            var cleanUrl = baseUrl.TrimEnd('/');
            var url = $"{cleanUrl}/api/calendar/check";

            // Cria o objeto anónimo para enviar no corpo do pedido (JSON)
            var payload = new { Start = start, End = end };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, payload);

                if (response.IsSuccessStatusCode)
                {
                    // Deserializa a resposta JSON numa lista de strings
                    var conflicts = await response.Content.ReadFromJsonAsync<List<string>>();
                    return conflicts ?? new List<string>();
                }
                else
                {
                    // Log interno para depuração (não visível ao utilizador)
                    Console.WriteLine($"Erro Calendário: {response.StatusCode}");
                    return new List<string>();
                }
            }
            catch (Exception)
            {
                // Em caso de erro de conexão, retorna lista vazia para não bloquear a aplicação
                return new List<string>();
            }
        }
    }
}