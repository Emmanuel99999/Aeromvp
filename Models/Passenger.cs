using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace Aeromvp.Models
{
    public class Passenger
    {
        // Clave primaria
        [Key]
        public int PassengerId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string DocumentType { get; set; } = null!; // CC, TI, PASSPORT, etc.

        [Required]
        [StringLength(30)]
        public string DocumentNumber { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        // Trazabilidad mínima
        [Required]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAtUtc { get; set; }


        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
