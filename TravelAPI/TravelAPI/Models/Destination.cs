using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAPI.Models
{
    public class Destination
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        public string Description { get; set; }

        public string ImageURL { get; set; }

        public string CountryCode { get; set; }

        [NotMapped]
        public string WeatherInfo { get; set; }
    }
}
