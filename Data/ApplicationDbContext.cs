using Aeromvp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq; // 👈 importante para SelectMany

namespace Aeromvp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets principales
        public DbSet<Passenger> Passengers { get; set; } = null!;
        public DbSet<Flight> Flights { get; set; } = null!;
        public DbSet<Leg> Legs { get; set; } = null!;
        public DbSet<Fare> Fares { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Seat> Seats { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // Booking <-> Payment (1:1)
            // =========================
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // Flight <-> Booking (1:N)
            // =========================
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Flight)
                .WithMany(f => f.Bookings)
                .HasForeignKey(b => b.FlightId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================
            // Ticket <-> Fare (1:N)
            // =========================
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Fare)
                .WithMany(f => f.Tickets)
                .HasForeignKey(t => t.FareId)
                .OnDelete(DeleteBehavior.NoAction);

            // =========================================
            // REGRA GENERAL: quitar Cascade en todo FK
            // =========================================
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys())
                         .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }
    }
}
