using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Data;
using TravelAPI.Models;

namespace TravelAPI.Controllers
{
    [Route("travel/package")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
       private readonly AppDbContext _context;

        public PackagesController(AppDbContext context)
        {
           _context = context;
        }

        //GET: api/Packages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Packages>>> GetPackages()
        {
            return await _context.Packages.ToListAsync();
        }

        // 2. GET: api/packages/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Packages>> GetPackageById(int id)
        {
           var package = await _context.Packages.FindAsync(id);

            if (package == null)
            {
                return NotFound();
            }
            return package;
        }

        // 3. POST: api/packages
        [HttpPost]
        public async Task<ActionResult<Packages>> PostPackage(Packages package)
        {
            _context.Packages.Add(package);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPackageById), new { id = package.Id }, package);
        }

        // 4. PUT: api/packages/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPackage(int id, Packages package)
        {
            if (id != package.Id)
            {
                return BadRequest();
            }

            _context.Entry(package).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PackageExists(id))
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

        // 5. DELETE: api/packages/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package == null)
            {
                return NotFound();
            }

            _context.Packages.Remove(package);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PackageExists(int id)
        {
            return _context.Packages.Any(e => e.Id == id);
        }
        
    }
}
