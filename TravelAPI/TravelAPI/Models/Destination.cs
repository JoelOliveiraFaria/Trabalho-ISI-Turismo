using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
    public class Destination
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Country { get; set; }
        public string? Description { get; set; }

        public List<Trip> Trips { get; set; }
    }
}
