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
        public string FlightNumber { get; set; } = null!;  

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

        [Range(1, 1000)]
        public int TotalSeats { get; set; }

        [StringLength(50)]
        public string? OperatingAirline { get; set; }

        [StringLength(50)]
        public string? MarketingAirline { get; set; }

        // Estado del vuelo
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Scheduled";
 

        // Marcas de tiempo
        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }


        public ICollection<Leg> Legs { get; set; } = new List<Leg>();
        public ICollection<Fare> Fares { get; set; } = new List<Fare>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
