using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aeromvp.Data;
using Aeromvp.Models;

namespace Aeromvp.Controllers
{
    public class FareController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FareController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fare
        public async Task<IActionResult> Index()
        {
            var fares = await _context.Fares
                .Include(f => f.Flight)
                .ToListAsync();

            return View(fares);
        }

        // GET: Fare/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var fare = await _context.Fares
                .Include(f => f.Flight)
                .FirstOrDefaultAsync(m => m.FareId == id);

            if (fare == null)
                return NotFound();

            return View(fare);
        }

        // GET: Fare/Create
        public IActionResult Create()
        {
            ViewData["Flights"] = _context.Flights.ToList();
            return View();
        }

        // POST: Fare/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Fare fare)
        {
            if (ModelState.IsValid)
            {
                fare.TotalPrice = fare.BaseFare + fare.Taxes + fare.AdditionalFees;
                _context.Add(fare);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Flights"] = _context.Flights.ToList();
            return View(fare);
        }

        // GET: Fare/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var fare = await _context.Fares.FindAsync(id);
            if (fare == null)
                return NotFound();

            ViewData["Flights"] = _context.Flights.ToList();
            return View(fare);
        }

        // POST: Fare/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Fare fare)
        {
            if (id != fare.FareId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    fare.TotalPrice = fare.BaseFare + fare.Taxes + fare.AdditionalFees;
                    _context.Update(fare);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Fares.Any(e => e.FareId == fare.FareId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["Flights"] = _context.Flights.ToList();
            return View(fare);
        }

        // GET: Fare/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var fare = await _context.Fares
                .Include(f => f.Flight)
                .FirstOrDefaultAsync(m => m.FareId == id);

            if (fare == null)
                return NotFound();

            return View(fare);
        }

        // POST: Fare/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fare = await _context.Fares.FindAsync(id);
            if (fare != null)
            {
                _context.Fares.Remove(fare);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
