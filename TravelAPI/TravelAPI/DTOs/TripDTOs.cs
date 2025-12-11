using System.ComponentModel.DataAnnotations;

namespace TravelAPI.DTOs
{
    public class CreateTripDto
    {
        [Required]
        public int DestinationId { get; set; } 

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal Budget { get; set; }
        public string Notes { get; set; }
    }

    public class TripDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Budget { get; set; }
        public string Notes { get; set; }
        public decimal? InsuranceCost { get; set; }
        public string? WeatherForecast { get; set; }
        public DestinationDto Destination { get; set; }
    }
}
