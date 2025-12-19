using System.Net.Http.Json;
using TravelAPI.Interfaces;

namespace TravelAPI.Services
{
    public class RemoteEmailService : IEmailService
    {
        private readonly HttpClient _httpClient;

        public RemoteEmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailData = new
            {
                ToEmail = toEmail,
                Subject = subject,
                Body = body
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/email/send", emailData);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"⚠️ O EmailService devolveu erro: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao contactar o microsserviço de Email: {ex.Message}");
            }
        }
    }
}