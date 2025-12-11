using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.DTOs;
using TravelAPI.Models;

namespace TravelAPI.Controllers
{
    [Route("travel/destinations")]
    [ApiController]
    public class DestinationsController : ControllerBase
    {
        private readonly TravelPlannerContext _context;

        public DestinationsController(TravelPlannerContext context)
        {
            _context = context;
        }

        // 1. OBTER TODOS OS DESTINOS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DestinationDto>>> GetDestinations([FromQuery] string? search)
        {
            var query = _context.Destinations.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.City.Contains(search) || d.Country.Contains(search));
            }

            var destinations = await query
                .Select(d => new DestinationDto
                {
                    Id = d.Id,
                    City = d.City,
                    Country = d.Country,
                    Description = d.Description
                })
                .ToListAsync();

            return Ok(destinations);
        }

        // 2. OBTER UM POR ID
        // GET: api/destinations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DestinationDto>> GetDestination(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);

            if (destination == null)
            {
                return NotFound();
            }

            // Converter Model -> DTO
            var destinationDto = new DestinationDto
            {
                Id = destination.Id,
                City = destination.City,
                Country = destination.Country,
                Description = destination.Description
            };

            return Ok(destinationDto);
        }

        // 3. CRIAR NOVO DESTINO
        // POST: api/destinations
        [HttpPost]
        public async Task<ActionResult<DestinationDto>> PostDestination(CreateDestinationDto createDto)
        {
            var destination = new Destination
            {
                City = createDto.City,
                Country = createDto.Country,
                Description = createDto.Description
            };

            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            var resultDto = new DestinationDto
            {
                Id = destination.Id,
                City = destination.City,
                Country = destination.Country,
                Description = destination.Description
            };

            return CreatedAtAction(nameof(GetDestination), new { id = destination.Id }, resultDto);
        }

        // 4. ATUALIZAR DESTINO

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestination(int id, CreateDestinationDto updateDto)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            destination.City = updateDto.City;
            destination.Country = updateDto.Country;
            destination.Description = updateDto.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 5. APAGAR DESTINO
        // DELETE: api/destinations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}