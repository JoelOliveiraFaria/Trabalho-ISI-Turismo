using System.ComponentModel.DataAnnotations;

namespace TravelAPI.DTOs
{
    public class CreateDestinationDto
    {
        [Required]
        public string? City { get; set; }

        [Required]
        public string? Country { get; set; }
        public string? Description { get; set; }
    }

    public class DestinationDto
    {
        public int Id { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
    }
}
