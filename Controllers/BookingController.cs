using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aeromvp.Data;
using Aeromvp.Models;

namespace Aeromvp.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Passenger)
                .Include(b => b.Flight)
                .Include(b => b.Fare)
                .ToListAsync();

            return View(bookings);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var booking = await _context.Bookings
                .Include(b => b.Passenger)
                .Include(b => b.Flight)
                .Include(b => b.Fare)
                .Include(b => b.Payment)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            ViewData["Passengers"] = _context.Passengers.ToList();
            ViewData["Flights"] = _context.Flights.ToList();
            ViewData["Fares"] = _context.Fares.ToList();
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Passengers"] = _context.Passengers.ToList();
                ViewData["Flights"] = _context.Flights.ToList();
                ViewData["Fares"] = _context.Fares.ToList();
                return View(booking);
            }

            // Genera código PNR seguro y corto
            booking.LocatorCode = GenerateLocatorCode();
            booking.CreatedAtUtc = DateTime.UtcNow;
            booking.ExpiresAtUtc = DateTime.UtcNow.AddMinutes(15); // reserva temporal

            _context.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            ViewData["Passengers"] = _context.Passengers.ToList();
            ViewData["Flights"] = _context.Flights.ToList();
            ViewData["Fares"] = _context.Fares.ToList();

            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewData["Passengers"] = _context.Passengers.ToList();
                ViewData["Flights"] = _context.Flights.ToList();
                ViewData["Fares"] = _context.Fares.ToList();
                return View(booking);
            }

            try
            {
                booking.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(booking);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(booking.BookingId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var booking = await _context.Bookings
                .Include(b => b.Passenger)
                .Include(b => b.Flight)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }

        private string GenerateLocatorCode()
        {
            // Código PNR de 6 caracteres, alfanumérico
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                 .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
