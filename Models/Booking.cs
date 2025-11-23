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

        // Relación con la tarifa seleccionada
        [Required]
        public int FareId { get; set; }

        public Fare Fare { get; set; } = null!;

        // Relación con vuelo o con tramo
        [Required]
        public int FlightId { get; set; }

        public Flight Flight { get; set; } = null!;

        // Pasajero asociado a la reserva
        [Required]
        public int PassengerId { get; set; }

        public Passenger Passenger { get; set; } = null!;

        // Valor total confirmado al momento de la reserva
        [Range(0, 999999)]
        public decimal TotalPrice { get; set; }

        // Fecha y hora de expiración de reserva (hold)
        public DateTime ExpiresAtUtc { get; set; }

        // Estado de la reserva
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";
        // Pending, Confirmed, Cancelled

        // Relación con pago
        public int? PaymentId { get; set; }

        public Payment? Payment { get; set; }

        // Marcas de auditoría
        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }

        // Navegación a tickets si la reserva se emite
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
