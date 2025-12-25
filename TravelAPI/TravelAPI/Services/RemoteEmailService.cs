/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: RemoteEmailService.cs
 * Descrição: Implementação do serviço de envio de emails (IEmailService).
 * Envia pedidos POST para um microsserviço externo de email.
 * ===================================================================================
 */

using System.Net.Http.Json;
using TravelAPI.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TravelAPI.Services
{
    /// <summary>
    /// Serviço que comunica com uma API Externa de Email via HTTP/REST.
    /// Utilizado para enviar notificações (ex: conflitos de agenda) aos utilizadores.
    /// </summary>
    public class RemoteEmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor que injeta o cliente HTTP e as configurações.
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para comunicação externa.</param>
        /// <param name="configuration">Acesso ao ficheiro appsettings.json.</param>
        public RemoteEmailService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        /// <summary>
        /// Envia um email de forma assíncrona para o serviço externo.
        /// </summary>
        /// <param name="toEmail">Email de destino.</param>
        /// <param name="subject">Assunto da mensagem.</param>
        /// <param name="body">Corpo da mensagem.</param>
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var baseUrl = _configuration["EmailServiceUrl"];

            // Validação de configuração
            if (string.IsNullOrEmpty(baseUrl))
            {
                Console.WriteLine("EmailServiceUrl não configurado.");
                return;
            }

            var cleanUrl = baseUrl.TrimEnd('/');
            var url = $"{cleanUrl}/api/email/send";

            var emailData = new { ToEmail = toEmail, Subject = subject, Body = body };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, emailData);

                if (!response.IsSuccessStatusCode)
                {
                    // Log de erro interno (apenas visível na consola do servidor)
                    Console.WriteLine($"Erro Email: {response.StatusCode}");
                }
            }
            catch (Exception)
            {
                // Falha silenciosa para não interromper o fluxo principal da aplicação
                Console.WriteLine("Exceção ao enviar email.");
            }
        }
    }
}