using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.Models;

namespace TravelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationsController : ControllerBase
    {
     private readonly AppDbContext _context;

        public DestinationsController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/Destinations
        //Devolve a lista de destinos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Destination>>> GetDestinations()
        {
            return await _context.Destinations.ToListAsync();
        }
        // 2. GET: api/destinations/{id}
        // Devolve um destino específico
        [HttpGet("{id}")]
        public async Task<ActionResult<Destination>> GetDestinationById(int id)
        {
           var destination = await _context.Destinations.FindAsync(id);

            if (destination == null)
            {
                return NotFound();
            }
            return destination;
        }

        // 3. POST: api/destinations
        [HttpPost]
        public async Task<ActionResult<Destination>> PostDestination(Destination destination)
        {
            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDestinationById), new { id = destination.Id }, destination);
        }

        // 4. PUT: api/destinations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestination(int id, Destination destination)
        {
            if (id != destination.Id)
            {
                return BadRequest();
            }
            _context.Entry(destination).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            var packages = _context.Packages.Where(p => p.DestinationId == id);
            _context.Packages.RemoveRange(packages);

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DestinationExists(int id)
        {
            return _context.Destinations.Any(e => e.Id == id);
        }
    }
}
