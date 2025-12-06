using Microsoft.EntityFrameworkCore;
using TravelAPI.Models;

namespace TravelAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Packages> Packages { get; set; }
    }
}
