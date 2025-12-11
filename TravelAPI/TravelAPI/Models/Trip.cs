using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAPI.Models
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Budget { get; set; }

        public string? Notes { get; set; }

        // --- CAMPOS PARA INTEGRAÇÃO (SOAP & REST Externo) ---
        public decimal? InsuranceCost { get; set; }

        public string? WeatherForecast { get; set; }

        // --- RELAÇÕES (Foreign Keys) ---
        public int UserId { get; set; }
        public User User { get; set; }

        public int DestinationId { get; set; }
        public Destination Destination { get; set; }
    }
}
