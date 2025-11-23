using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Aeromvp.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }

        // Tramo al que pertenece este asiento
        [Required]
        public int LegId { get; set; }

        public Leg Leg { get; set; } = null!;

        // Ej: 12A, 5C
        [Required]
        [StringLength(5)]
        public string SeatNumber { get; set; } = null!;

        // Clase de cabina: Economy, Premium, Business, First
        [StringLength(20)]
        public string? CabinClass { get; set; }

        // Tipo de asiento: Window, Aisle, Middle, ExitRow, etc.
        [StringLength(20)]
        public string? SeatType { get; set; }

        // Marca si el asiento está disponible para ser seleccionado
        public bool IsAvailable { get; set; } = true;

        // Marca si la aerolínea lo bloquea (tripulación, mantenimiento, etc.)
        public bool IsBlocked { get; set; }

        // Cargo extra por seleccionar este asiento (si aplica)
        [Range(0, 999999)]
        public decimal ExtraFee { get; set; }

        // Auditoría
        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }

        // Si quieres relacionar asiento con tickets que lo usan
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
