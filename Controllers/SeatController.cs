using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aeromvp.Data;
using Aeromvp.Models;

namespace Aeromvp.Controllers
{
    public class SeatController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SeatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Seat
        public async Task<IActionResult> Index()
        {
            var seats = await _context.Seats
                .Include(s => s.Leg)
                .ToListAsync();

            return View(seats);
        }

        // GET: Seat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var seat = await _context.Seats
                .Include(s => s.Leg)
                .FirstOrDefaultAsync(m => m.SeatId == id);

            if (seat == null) return NotFound();

            return View(seat);
        }

        // GET: Seat/Create
        public IActionResult Create()
        {
            ViewData["LegId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Legs, "LegId", "LegId");

            return View();
        }

        // POST: Seat/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seat seat)
        {
            if (!ModelState.IsValid)
            {
                ViewData["LegId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                    _context.Legs, "LegId", "LegId", seat.LegId);

                return View(seat);
            }

            _context.Add(seat);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Seat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var seat = await _context.Seats.FindAsync(id);
            if (seat == null) return NotFound();

            ViewData["LegId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Legs, "LegId", "LegId", seat.LegId);

            return View(seat);
        }

        // POST: Seat/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seat seat)
        {
            if (id != seat.SeatId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["LegId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                    _context.Legs, "LegId", "LegId", seat.LegId);

                return View(seat);
            }

            try
            {
                _context.Update(seat);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Seats.Any(e => e.SeatId == seat.SeatId))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Seat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var seat = await _context.Seats
                .Include(s => s.Leg)
                .FirstOrDefaultAsync(m => m.SeatId == id);

            if (seat == null) return NotFound();

            return View(seat);
        }

        // POST: Seat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat != null)
            {
                _context.Seats.Remove(seat);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
