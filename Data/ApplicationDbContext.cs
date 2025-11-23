using Aeromvp.Models;
using Microsoft.EntityFrameworkCore;

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

            // Relación 1:1 Booking-Payment con Payment como dependiente (FK BookingId)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Payment)
                .WithOne(p => p.Booking)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
