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
    [Authorize] // Requer autenticação para aceder a este controlador
    public class TripsController : ControllerBase
    {
        private readonly TravelPlannerContext _context;
        private readonly IWeatherService _weatherService;

        public TripsController(TravelPlannerContext context, IWeatherService weatherService)
        {
            _context = context;
            _weatherService = weatherService;
        }

        // 1. OBTER TODOS AS VIAGENS
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

        // 2. OBTER UMA VIAGEM POR ID
        // GET: api/trips/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripDto>> GetTrip(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);

            var trip = await _context.Trips
                .Include(t => t.Destination)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId); 

            if (trip == null) return NotFound("Viagem não encontrada ou sem permissão de acesso.");

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


        [HttpPost]
        public async Task<ActionResult<TripDto>> PostTrip(CreateTripDto createDto)
        {
            // Validar se o destino existe antes de criar
            var destination = await _context.Destinations.FindAsync(createDto.DestinationId);
            if (destination == null)
            {
                return BadRequest("O Destino indicado não existe.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            string forecast = await _weatherService.GetWeatherAsync(destination.City!);

            if (!await _context.Users.AnyAsync(u => u.Id == userId))
            {
                return BadRequest("Utilizador (ID 1) não encontrado. Crie um user primeiro.");
            }

            var trip = new Trip
            {
                DestinationId = createDto.DestinationId,
                UserId = userId,
                StartDate = createDto.StartDate,
                EndDate = createDto.EndDate,
                Budget = createDto.Budget,
                Notes = createDto.Notes

                // --- FUTURO: É AQUI QUE CHAMAREMOS O SOAP E A API DE TEMPO ---
                // trip.InsuranceCost = await _soapService.Calculate(...);
            };

            trip.WeatherForecast = forecast;

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
                Destination = new DestinationDto
                {
                    Id = destination.Id,
                    City = destination.City,
                    Country = destination.Country
                }
            };

            return CreatedAtAction(nameof(GetTrip), new { id = trip.Id }, tripResponse);
        }

        // 4. APAGAR VIAGEM
        // DELETE: api/trips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();

            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
