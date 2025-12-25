/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: CalendarController.cs
 * Descrição: Controlador do Microsserviço de Calendário.
 * Expõe endpoints para verificar disponibilidade e conflitos de agenda (Google Calendar).
 * ===================================================================================
 */

using CalendarService.Services;
using Microsoft.AspNetCore.Mvc;
using CalendarService.Models;

namespace CalendarService.Controllers
{
    /// <summary>
    /// Controlador responsável pela gestão de eventos de calendário.
    /// Atua como interface para o serviço da Google Calendar API.
    /// </summary>
    [Route("api/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly GoogleCalendarService _googleService;

        /// <summary>
        /// Construtor que injeta o serviço de lógica do Google Calendar.
        /// </summary>
        /// <param name="googleService">Serviço de acesso à API da Google.</param>
        public CalendarController(GoogleCalendarService googleService)
        {
            _googleService = googleService;
        }

        /// <summary>
        /// Verifica se existem conflitos de agenda num intervalo de datas específico.
        /// </summary>
        /// <param name="request">Objeto contendo a data de início e fim a verificar.</param>
        /// <returns>Lista de conflitos encontrados (strings) ou lista vazia.</returns>
        // POST: api/calendar/check
        [HttpPost("check")]
        public async Task<IActionResult> CheckConflicts([FromBody] DateRangeRequest request)
        {
            // Chama o serviço para obter a lista de eventos que colidem com as datas
            var conflicts = await _googleService.CheckConflictsAsync(request.Start, request.End);

            // Retorna 200 OK com a lista (mesmo que vazia)
            return Ok(conflicts);
        }
    }
}