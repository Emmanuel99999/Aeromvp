using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aeromvp.Data;
using Aeromvp.Models;

namespace Aeromvp.Controllers
{
    public class LegController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LegController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Leg
        public async Task<IActionResult> Index()
        {
            var legs = await _context.Legs
                .Include(l => l.Flight)
                .ToListAsync();

            return View(legs);
        }

        // GET: Leg/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var leg = await _context.Legs
                .Include(l => l.Flight)
                .FirstOrDefaultAsync(l => l.LegId == id);

            if (leg == null)
                return NotFound();

            return View(leg);
        }

        // GET: Leg/Create
        public IActionResult Create()
        {
            ViewBag.Flights = _context.Flights.ToList();
            return View();
        }

        // POST: Leg/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Leg leg)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Flights = _context.Flights.ToList();
                return View(leg);
            }

            _context.Add(leg);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Leg/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var leg = await _context.Legs.FindAsync(id);
            if (leg == null)
                return NotFound();

            ViewBag.Flights = _context.Flights.ToList();
            return View(leg);
        }

        // POST: Leg/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Leg leg)
        {
            if (id != leg.LegId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                ViewBag.Flights = _context.Flights.ToList();
                return View(leg);
            }

            _context.Update(leg);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Leg/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var leg = await _context.Legs
                .Include(l => l.Flight)
                .FirstOrDefaultAsync(l => l.LegId == id);

            if (leg == null)
                return NotFound();

            return View(leg);
        }

        // POST: Leg/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leg = await _context.Legs.FindAsync(id);
            if (leg == null)
                return NotFound();

            _context.Legs.Remove(leg);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
