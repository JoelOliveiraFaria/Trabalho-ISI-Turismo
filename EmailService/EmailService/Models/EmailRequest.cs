/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: EmailRequest.cs
 * Descrição: Modelo de dados (DTO) utilizado para transportar a informação
 * necessária para o envio de um email (Destinatário, Assunto, Corpo).
 * ===================================================================================
 */

namespace EmailService.Models
{
    /// <summary>
    /// Objeto de pedido (Request Payload) para o envio de um email.
    /// </summary>
    public class EmailRequest
    {
        /// <summary>
        /// Endereço de email do destinatário.
        /// </summary>
        public string ToEmail { get; set; } = string.Empty;

        /// <summary>
        /// Assunto do email.
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Corpo ou conteúdo da mensagem.
        /// </summary>
        public string Body { get; set; } = string.Empty;
    }
}