using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
    public class Clients
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? TaxId { get; set; }

    }

    public class ClientsDTO
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? TaxId { get; set; }
    }
}
