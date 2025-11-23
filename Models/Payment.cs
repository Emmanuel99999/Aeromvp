using System;
using System.ComponentModel.DataAnnotations;

namespace Aeromvp.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        // Relación con la reserva
        [Required]
        public int BookingId { get; set; }

        public Booking Booking { get; set; } = null!;

        // Monto final cobrado
        [Range(0, 999999)]
        public decimal Amount { get; set; }

        // Método de pago
        [Required]
        [StringLength(20)]
        public string Method { get; set; } = null!;
        // Ej: Card, PSE, Paypal (simulado)

        // Estado del pago
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";
        // Pending, Approved, Rejected, Refunded

        // Código de confirmación entregado por la pasarela
        [StringLength(100)]
        public string? TransactionReference { get; set; }

        // Últimos 4 dígitos de la tarjeta (si aplica)
        [StringLength(4)]
        public string? CardLast4 { get; set; }

        // Antifraude / registro
        [StringLength(50)]
        public string? ClientIp { get; set; }

        // Auditoría
        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }
    }
}
