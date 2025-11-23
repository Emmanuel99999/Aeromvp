using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aeromvp.Data;
using Aeromvp.Models;

namespace Aeromvp.Controllers
{
    public class TicketController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ticket
        public async Task<IActionResult> Index()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Passenger)
                .Include(t => t.Flight)
                .Include(t => t.Booking)
                .ToListAsync();

            return View(tickets);
        }

        // GET: Ticket/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _context.Tickets
                .Include(t => t.Passenger)
                .Include(t => t.Flight)
                .Include(t => t.Booking)
                .FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null) return NotFound();

            return View(ticket);
        }

        // GET: Ticket/Create
        public IActionResult Create()
        {
            ViewData["Passengers"] = _context.Passengers.ToList();
            ViewData["Flights"] = _context.Flights.ToList();
            ViewData["Bookings"] = _context.Bookings.ToList();

            return View();
        }

        // POST: Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Passengers"] = _context.Passengers.ToList();
                ViewData["Flights"] = _context.Flights.ToList();
                ViewData["Bookings"] = _context.Bookings.ToList();
                return View(ticket);
            }

            ticket.Status = "Issued";
            ticket.IssuedAtUtc = DateTime.UtcNow;
            ticket.CreatedAtUtc = DateTime.UtcNow;

            _context.Add(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Ticket/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            ViewData["Passengers"] = _context.Passengers.ToList();
            ViewData["Flights"] = _context.Flights.ToList();
            ViewData["Bookings"] = _context.Bookings.ToList();

            return View(ticket);
        }

        // POST: Ticket/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.TicketId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["Passengers"] = _context.Passengers.ToList();
                ViewData["Flights"] = _context.Flights.ToList();
                ViewData["Bookings"] = _context.Bookings.ToList();
                return View(ticket);
            }

            try
            {
                ticket.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(ticket);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tickets.Any(e => e.TicketId == id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Ticket/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ticket = await _context.Tickets
                .Include(t => t.Passenger)
                .Include(t => t.Flight)
                .FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null) return NotFound();

            return View(ticket);
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Ticket/ChangeStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, string newStatus)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) return NotFound();

            // Validación simple de ciclo de vida
            var allowed = new[] { "Pending", "Issued", "Used", "Refunded" };
            if (!allowed.Contains(newStatus)) return BadRequest("Estado inválido.");

            ticket.Status = newStatus;
            ticket.UpdatedAtUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = ticket.TicketId });
        }
    }
}
