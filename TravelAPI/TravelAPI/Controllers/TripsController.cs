/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: TripsController.cs
 * Descrição: Controlador responsável pela gestão (CRUD) das viagens, incluindo
 * integração com serviços externos (Meteorologia, Calendário, Seguro, Email).
 * ===================================================================================
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelAPI.Data;
using TravelAPI.DTOs;
using TravelAPI.Interfaces;
using TravelAPI.Models;

namespace TravelAPI.Controllers
{
    [Route("travel/trips")]
    [ApiController]
    [Authorize]
    public class TripsController : ControllerBase
    {
        private readonly TravelPlannerContext _context;
        private readonly IWeatherService _weatherService;
        private readonly IInsuranceService _insuranceService;
        private readonly ICalendarService _calendarService;
        private readonly IEmailService _emailService; 

        public TripsController(
            TravelPlannerContext context,
            IWeatherService weatherService,
            IInsuranceService insuranceService,
            ICalendarService calendarService,
            IEmailService emailService) 
        {
            _context = context;
            _weatherService = weatherService;
            _insuranceService = insuranceService;
            _calendarService = calendarService;
            _emailService = emailService;
        }

        /// <summary>
        /// Obtém todas as viagens associadas ao utilizador autenticado.
        /// </summary>
        /// <returns>Lista de viagens (TripDto).</returns>
        // 1. OBTER TODAS AS VIAGENS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripDto>>> GetTrips()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var trips = await _context.Trips
                .Where(t => t.UserId == userId)
                .Include(t => t.Destination)
                .Select(t => new TripDto
                {
                    Id = t.Id,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    Budget = t.Budget,
                    Notes = t.Notes!,
                    WeatherForecast = t.WeatherForecast,
                    InsuranceCost = t.InsuranceCost,
                    Destination = new DestinationDto
                    {
                        Id = t.Destination.Id,
                        City = t.Destination.City,
                        Country = t.Destination.Country,
                        Description = t.Destination.Description
                    }
                })
                .ToListAsync();

            return Ok(trips);
        }

        /// <summary>
        /// Obtém os detalhes de uma viagem específica.
        /// Verifica se a viagem pertence ao utilizador autenticado.
        /// </summary>
        /// <param name="id">ID da viagem.</param>
        /// <returns>Detalhes da viagem ou NotFound.</returns>
        // 2. OBTER UMA VIAGEM POR ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TripDto>> GetTrip(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var trip = await _context.Trips
                .Include(t => t.Destination)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (trip == null) return NotFound();

            var tripDto = new TripDto
            {
                Id = trip.Id,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                Budget = trip.Budget,
                Notes = trip.Notes!,
                InsuranceCost = trip.InsuranceCost,
                WeatherForecast = trip.WeatherForecast,
                Destination = new DestinationDto
                {
                    Id = trip.Destination.Id,
                    City = trip.Destination.City,
                    Country = trip.Destination.Country,
                    Description = trip.Destination.Description
                }
            };

            return Ok(tripDto);
        }

        /// <summary>
        /// Cria uma nova viagem.
        /// Executa integrações com serviços externos: Meteorologia, Seguro e Calendário.
        /// Envia alerta por email se houver conflitos de agenda.
        /// </summary>
        /// <param name="createDto">Dados da nova viagem.</param>
        /// <returns>A viagem criada ou mensagem de aviso de conflitos.</returns>
        // 3. CRIAR VIAGEM
        [HttpPost]
        public async Task<IActionResult> PostTrip(CreateTripDto createDto)
        {
            // Validação de Data
            if (createDto.EndDate < createDto.StartDate)
            {
                return BadRequest();
            }

            // Validar se o destino existe
            var destination = await _context.Destinations.FindAsync(createDto.DestinationId);
            if (destination == null)
            {
                return BadRequest();
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }

            var conflicts = await _calendarService.CheckConflictsAsync(createDto.StartDate, createDto.EndDate);
            string forecast = await _weatherService.GetWeatherAsync(destination.City!);
            decimal insurance = await _insuranceService.CalculateInsuranceAsync(createDto.Budget);

            var trip = new Trip
            {
                DestinationId = createDto.DestinationId,
                UserId = userId,
                StartDate = createDto.StartDate,
                EndDate = createDto.EndDate,
                Budget = createDto.Budget,
                Notes = createDto.Notes,
                InsuranceCost = insurance,
                WeatherForecast = forecast
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            var tripResponse = new TripDto
            {
                Id = trip.Id,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                Budget = trip.Budget,
                Notes = trip.Notes,
                WeatherForecast = forecast,
                InsuranceCost = trip.InsuranceCost,
                Destination = new DestinationDto
                {
                    Id = destination.Id,
                    City = destination.City,
                    Country = destination.Country
                }
            };

            if (conflicts != null && conflicts.Any())
            {
                string listaConflitos = string.Join(", ", conflicts);

                string assunto = $"Conflito na Viagem para {destination.City}";
                string corpo = $"Olá {user.Username},\n\n" +
                               $"Criaste uma viagem para {destination.City} de {createDto.StartDate:dd/MM} a {createDto.EndDate:dd/MM}.\n" +
                               $"No entanto, detetámos os seguintes conflitos na tua agenda:\n\n" +
                               $"{listaConflitos}\n\n" +
                               $"Verifica a tua disponibilidade!";

                _ = _emailService.SendEmailAsync(user.Email!, assunto, corpo);

                return Ok(new
                {
                    Message = "Viagem criada com sucesso, mas atenção aos conflitos na agenda! Foi enviado um email de aviso.",
                    Warnings = conflicts,
                    TripData = tripResponse
                });
            }

            return CreatedAtAction(nameof(GetTrip), new { id = trip.Id }, tripResponse);
        }

        /// <summary>
        /// Atualiza uma viagem existente.
        /// Recalcula automaticamente o valor do seguro com base no novo orçamento.
        /// </summary>
        /// <param name="id">ID da viagem.</param>
        /// <param name="tripDto">Novos dados da viagem.</param>
        /// <returns>NoContent em caso de sucesso.</returns>
        // 4. ATUALIZAR VIAGEM
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrip(int id, CreateTripDto tripDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (trip == null) return NotFound();

            trip.StartDate = tripDto.StartDate;
            trip.EndDate = tripDto.EndDate;
            trip.Budget = tripDto.Budget;
            trip.Notes = tripDto.Notes;
            trip.InsuranceCost = await _insuranceService.CalculateInsuranceAsync(trip.Budget);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Remove uma viagem do sistema.
        /// </summary>
        /// <param name="id">ID da viagem a remover.</param>
        /// <returns>NoContent em caso de sucesso.</returns>
        // 5. APAGAR VIAGEM
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var trip = await _context.Trips
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (trip == null) return NotFound();

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}