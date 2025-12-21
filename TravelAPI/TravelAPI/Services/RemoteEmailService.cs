using System.Net.Http.Json;
using TravelAPI.Interfaces;
using Microsoft.Extensions.Configuration;

namespace TravelAPI.Services
{
    public class RemoteEmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public RemoteEmailService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var baseUrl = _configuration["EmailServiceUrl"];

            if (string.IsNullOrEmpty(baseUrl))
            {
                Console.WriteLine("ERRO: EmailServiceUrl não está configurado no Azure/appsettings!");
                return;
            }

            var url = $"{baseUrl}/api/email/send";

            var emailData = new
            {
                ToEmail = toEmail,
                Subject = subject,
                Body = body
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, emailData);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"O EmailService devolveu erro: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao contactar o microsserviço de Email: {ex.Message}");
            }
        }
    }
}