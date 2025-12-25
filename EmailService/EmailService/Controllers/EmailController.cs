/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: EmailController.cs
 * Descrição: Controlador do Microsserviço de Email.
 * Recebe pedidos HTTP para envio de correio eletrónico via SMTP (Gmail).
 * ===================================================================================
 */

using EmailService.Models;
using EmailService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Controller
{
    /// <summary>
    /// Controlador responsável pela exposição da funcionalidade de envio de emails.
    /// </summary>
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {
        private readonly GmailService _gmailService;

        /// <summary>
        /// Construtor que injeta o serviço de envio de emails (Gmail).
        /// </summary>
        /// <param name="gmailService">Serviço de lógica de SMTP.</param>
        public EmailController(GmailService gmailService)
        {
            _gmailService = gmailService;
        }

        /// <summary>
        /// Endpoint para enviar um email.
        /// Recebe os dados (Destinatário, Assunto, Corpo) e invoca o serviço SMTP.
        /// </summary>
        /// <param name="request">Dados do email a enviar.</param>
        /// <returns>Confirmação de processamento.</returns>
        // POST: api/email/send
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            await _gmailService.SendEmailAsync(request.ToEmail, request.Subject, request.Body);
            return Ok();
        }
    }
}