using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace Aeromvp.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        // Código localizador (PNR)
        [Required]
        [StringLength(8, MinimumLength = 5)]
        public string LocatorCode { get; set; } = null!;

        [Required]
        public int FareId { get; set; }

        public Fare Fare { get; set; } = null!;


        [Required]
        public int FlightId { get; set; }

        public Flight Flight { get; set; } = null!;

        [Required]
        public int PassengerId { get; set; }

        public Passenger Passenger { get; set; } = null!;


        [Range(0, 999999)]
        public decimal TotalPrice { get; set; }


        public DateTime ExpiresAtUtc { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        public int? PaymentId { get; set; }

        public Payment? Payment { get; set; }


        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }

        // Navegación a tickets si la reserva se emite
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
