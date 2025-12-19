using EmailService.Models;
using EmailService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Controller
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {
        private readonly GmailService _gmailService;

        public EmailController(GmailService gmailService)
        {
            _gmailService = gmailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            await _gmailService.SendEmailAsync(request.ToEmail, request.Subject, request.Body);
            return Ok("Email enviado (ou tentado)!");
        }
    }
}
