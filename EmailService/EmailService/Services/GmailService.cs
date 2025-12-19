using System.Net.Mail;
using System.Net;

namespace EmailService.Services
{
    public class GmailService
    {
        private readonly string _myEmail;
        private readonly string _myPassword;

        public GmailService(IConfiguration configuration)
        {
            _myEmail = configuration["EmailSettings:Email"]!;
            _myPassword = configuration["EmailSettings:Password"]!;
        }

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
                Console.WriteLine(ex.Message);
            }
        }
    }
}
