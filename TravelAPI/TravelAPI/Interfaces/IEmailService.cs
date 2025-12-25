/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: IEmailService.cs
 * Descrição: Interface que define o contrato para o serviço de envio de emails.
 * Permite notificar os utilizadores sobre eventos (ex: conflitos de agenda).
 * ===================================================================================
 */

namespace TravelAPI.Interfaces
{
    /// <summary>
    /// Define o contrato para serviços de envio de correio eletrónico.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Envia um email de forma assíncrona.
        /// </summary>
        /// <param name="to">Endereço de email do destinatário.</param>
        /// <param name="subject">Assunto do email.</param>
        /// <param name="body">Corpo/Conteúdo da mensagem.</param>
        Task SendEmailAsync(string to, string subject, string body);
    }
}