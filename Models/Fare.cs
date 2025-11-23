using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace Aeromvp.Models
{
    public class Fare
    {
        [Key]
        public int FareId { get; set; }

        // Relación con el vuelo al que pertenece la tarifa
        [Required]
        public int FlightId { get; set; }

        public Flight Flight { get; set; } = null!;

        // Nombre o tipo de tarifa (ej: Basic, Standard, Flex)
        [Required]
        [StringLength(30)]
        public string FareType { get; set; } = null!;

        // Precio base de la tarifa 
        [Range(0, 999999)]
        public decimal BaseFare { get; set; }

        // Impuestos
        [Range(0, 999999)]
        public decimal Taxes { get; set; }

        // Cargos adicionales fijos (ej: tasas aeroportuarias)
        [Range(0, 999999)]
        public decimal AdditionalFees { get; set; }

        // Precio total calculado (se puede recalcular en servicio)
        [Range(0, 999999)]
        public decimal TotalPrice { get; set; }

        // Clase de cabina: Economy, Premium, Business, First
        [StringLength(20)]
        public string? CabinClass { get; set; }

        // Políticas básicas
        public bool AllowRefund { get; set; }
        public bool AllowChanges { get; set; }

        // Validaciones de seguridad
        [StringLength(200)]
        public string? Conditions { get; set; }

        // Marcas de tiempo
        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }

        // Navegación
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
