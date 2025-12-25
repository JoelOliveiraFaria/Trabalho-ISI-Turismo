/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: GmailService.cs
 * Descrição: Serviço responsável pelo envio real de emails usando o servidor SMTP
 * da Google (Gmail). Utiliza credenciais (App Password) do ficheiro de configuração.
 * ===================================================================================
 */

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace EmailService.Services
{
    /// <summary>
    /// Implementação do serviço de envio de email utilizando o SMTP da Google.
    /// </summary>
    public class GmailService
    {
        private readonly string _myEmail;
        private readonly string _myPassword;

        /// <summary>
        /// Construtor que obtém as credenciais de email do ficheiro de configuração.
        /// </summary>
        /// <param name="configuration">Acesso ao appsettings.json.</param>
        public GmailService(IConfiguration configuration)
        {
            _myEmail = configuration["EmailSettings:Email"]!;
            _myPassword = configuration["EmailSettings:Password"]!;
        }

        /// <summary>
        /// Configura o cliente SMTP e envia o email de forma assíncrona.
        /// </summary>
        /// <param name="toEmail">Email do destinatário.</param>
        /// <param name="subject">Assunto do email.</param>
        /// <param name="body">Corpo do email (pode ser HTML).</param>
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_myEmail, _myPassword);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_myEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true,
                    };

                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Log de erro na consola do servidor para despiste
                Console.WriteLine(ex.Message);
            }
        }
    }
}