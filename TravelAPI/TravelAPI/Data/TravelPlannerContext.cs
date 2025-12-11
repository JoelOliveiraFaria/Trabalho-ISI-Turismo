using Microsoft.EntityFrameworkCore;
using TravelAPI.Models;

namespace TravelAPI.Data
{
    public class TravelPlannerContext : DbContext
    {
        public TravelPlannerContext(DbContextOptions<TravelPlannerContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Trip> Trips { get; set; }
    }
}
