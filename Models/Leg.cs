using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace Aeromvp.Models
{
    public class Leg
    {
        [Key]
        public int LegId { get; set; }

        // Relación con el vuelo “padre”
        [Required]
        public int FlightId { get; set; }

        public Flight Flight { get; set; } = null!;

        // Número de tramo dentro del itinerario (1, 2, 3…)
        [Range(1, 10)]
        public int SegmentNumber { get; set; }

        [Required]
        [StringLength(3)]
        public string OriginAirportCode { get; set; } = null!;

        [Required]
        [StringLength(3)]
        public string DestinationAirportCode { get; set; } = null!;

        [Required]
        public DateTime DepartureTimeUtc { get; set; }

        [Required]
        public DateTime ArrivalTimeUtc { get; set; }

        [Range(1, 2000)]
        public int DurationMinutes { get; set; }

        // Clase de cabina principal del tramo (puede haber mezcla en otra tabla si quieres)
        [StringLength(20)]
        public string? CabinClass { get; set; } // Economy, Business, etc.

        // Capacidad de asientos solo de este tramo
        [Range(1, 1000)]
        public int AvailableSeats { get; set; }

        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }

        // Navegación
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
