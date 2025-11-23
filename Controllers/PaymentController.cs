using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aeromvp.Data;
using Aeromvp.Models;

namespace Aeromvp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payment
        public async Task<IActionResult> Index()
        {
            var payments = await _context.Payments
                .Include(p => p.Booking)
                .ToListAsync();

            return View(payments);
        }

        // GET: Payment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payments
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(m => m.PaymentId == id);

            if (payment == null) return NotFound();

            return View(payment);
        }

        // GET: Payment/Create
        public IActionResult Create(int? bookingId)
        {
            if (bookingId == null)
                return BadRequest("bookingId is required.");

            ViewData["BookingId"] = bookingId;
            return View();
        }

        // POST: Payment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Payment payment)
        {
            if (!ModelState.IsValid)
            {
                ViewData["BookingId"] = payment.BookingId;
                return View(payment);
            }

            // Validación extra: el booking debe existir
            var booking = await _context.Bookings.FindAsync(payment.BookingId);
            if (booking == null)
            {
                ModelState.AddModelError("", "Booking not found.");
                return View(payment);
            }

            payment.CreatedAtUtc = DateTime.UtcNow;

            _context.Add(payment);

            // Si el pago se aprueba, actualizamos la reserva
            if (payment.Status == "Approved")
            {
                booking.Status = "Confirmed";
                booking.PaymentId = payment.PaymentId;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Payment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();

            return View(payment);
        }

        // POST: Payment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Payment payment)
        {
            if (id != payment.PaymentId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(payment);

            try
            {
                payment.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(payment);

                // Validación adicional: si se marca Approved, actualizar reserva
                var booking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.BookingId == payment.BookingId);

                if (booking != null)
                {
                    if (payment.Status == "Approved")
                        booking.Status = "Confirmed";
                    else if (payment.Status == "Rejected")
                        booking.Status = "Pending";
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(payment.PaymentId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Payment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var payment = await _context.Payments
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(m => m.PaymentId == id);

            if (payment == null) return NotFound();

            return View(payment);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
