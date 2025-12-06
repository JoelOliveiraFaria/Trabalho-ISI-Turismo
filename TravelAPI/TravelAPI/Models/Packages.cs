using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TravelAPI.Models
{
    public class Packages
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        // MUDANÇA AQUI: De 'Price' para 'BasePrice' para bater certo com o SQL
        public decimal BasePrice { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int DestinationId { get; set; }

        [ForeignKey("DestinationId")]
        public Destination? Destination { get; set; }
    }
}
