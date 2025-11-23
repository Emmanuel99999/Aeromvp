using System;
using System.ComponentModel.DataAnnotations;

namespace Aeromvp.ViewModels
{
    public class FlightSearchInputViewModel
    {
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Usa el código IATA (3 letras)")]
        public string OriginCode { get; set; } = string.Empty;

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Usa el código IATA (3 letras)")]
        public string DestinationCode { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        [Range(1, 9)]
        public int Adults { get; set; } = 1;

        [Range(0, 9)]
        public int Children { get; set; }

        [Range(0, 9)]
        public int Infants { get; set; }
    }
}
