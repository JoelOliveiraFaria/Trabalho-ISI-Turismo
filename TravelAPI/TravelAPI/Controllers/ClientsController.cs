using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.Models;

namespace TravelAPI.Controllers
{
    [Route("travel/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/clients

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clients>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // 2. GET: api/clients/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Clients>> GetClientById(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }
            return client;
        }

        // 3. POST: api/clients
        [HttpPost]
        public async Task<ActionResult<Clients>> PostClient(ClientsDTO clientdto)
        {
            var newClient = new Clients
            {
                Name = clientdto.Name,
                Email = clientdto.Email,
                Password = clientdto.Password,
                TaxId = clientdto.TaxId
            };

            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClientById), new { id = newClient.Id }, newClient);
        }

        // 4. PUT: api/clients/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, ClientsDTO clientdto)
        {
            var existingClient = await _context.Clients.FindAsync(id);

            if (existingClient == null)
            {
                return NotFound();
            }

            existingClient.Name = clientdto.Name;
            existingClient.Email = clientdto.Email;
            existingClient.Password = clientdto.Password;
            existingClient.TaxId = clientdto.TaxId;
                       
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // 5. DELETE: api/clients/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
