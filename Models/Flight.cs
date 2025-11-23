using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace Aeromvp.Models
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }

        [Required]
        [StringLength(10)]
        public string FlightNumber { get; set; } = null!;  // Ej: AV45, LA2330

        [Required]
        [StringLength(3)]
        public string OriginAirportCode { get; set; } = null!;  // IATA: MDE, BOG

        [Required]
        [StringLength(3)]
        public string DestinationAirportCode { get; set; } = null!;

        [Required]
        public DateTime DepartureTimeUtc { get; set; }

        [Required]
        public DateTime ArrivalTimeUtc { get; set; }

        // Duración calculada con respaldo en BD para optimizar búsquedas:
        [Range(1, 2000)]
        public int DurationMinutes { get; set; }

        // Capacidad total (ej: 150)
        [Range(1, 1000)]
        public int TotalSeats { get; set; }

        // Aerolínea operadora (por si es vuelo compartido/código compartido)
        [StringLength(50)]
        public string? OperatingAirline { get; set; }

        // Aerolínea comercial (cuando es diferente de la operadora)
        [StringLength(50)]
        public string? MarketingAirline { get; set; }

        // Estado del vuelo
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Scheduled";
        // Estados típicos: Scheduled, Delayed, Cancelled

        // Marcas de tiempo
        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }

        // Relaciones
        public ICollection<Leg> Legs { get; set; } = new List<Leg>();
        public ICollection<Fare> Fares { get; set; } = new List<Fare>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
