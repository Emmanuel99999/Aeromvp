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

        // Si luego quieres configurar algo extra:
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        // }
    }
}
