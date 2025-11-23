using System;
using System.ComponentModel.DataAnnotations;

namespace Aeromvp.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        // Código único del ticket
        [Required]
        [StringLength(12, MinimumLength = 6)]
        public string TicketNumber { get; set; } = null!;

        // Pasajero dueño del ticket
        [Required]
        public int PassengerId { get; set; }
        public Passenger Passenger { get; set; } = null!;

        // Reserva asociada
        [Required]
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        // Vuelo asignado
        [Required]
        public int FlightId { get; set; }
        public Flight Flight { get; set; } = null!;

        // Tarifa usada al momento de emisión
        [Required]
        public int FareId { get; set; }
        public Fare Fare { get; set; } = null!;

        // Estado del ticket
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Issued";
        // Posibles: Pending, Issued, Used, Refunded

        // Precio final congelado al momento de generación
        [Range(0, 999999)]
        public decimal FinalPrice { get; set; }

        // Correo donde se envía el ticket
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string DeliveryEmail { get; set; } = null!;

        // Auditorías + consistencia con controladores
        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // Fecha de emisión real
        [Required]
        public DateTime IssuedAtUtc { get; set; } = DateTime.UtcNow;

        // Última modificación
        public DateTime? UpdatedAtUtc { get; set; }
    }
}
