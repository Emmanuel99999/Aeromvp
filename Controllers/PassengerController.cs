using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aeromvp.Data;
using Aeromvp.Models;

namespace Aeromvp.Controllers
{
    public class PassengerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PassengerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Passenger
        public async Task<IActionResult> Index()
        {
            var passengers = await _context.Passengers
                .OrderBy(p => p.LastName)
                .ToListAsync();

            return View(passengers);
        }

        // GET: Passenger/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var passenger = await _context.Passengers
                .FirstOrDefaultAsync(m => m.PassengerId == id);

            if (passenger == null)
                return NotFound();

            return View(passenger);
        }

        // GET: Passenger/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Passenger/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Passenger passenger)
        {
            if (!ModelState.IsValid)
                return View(passenger);

            _context.Add(passenger);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Passenger/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var passenger = await _context.Passengers
                .FindAsync(id);

            if (passenger == null)
                return NotFound();

            return View(passenger);
        }

        // POST: Passenger/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Passenger passenger)
        {
            if (id != passenger.PassengerId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(passenger);

            try
            {
                passenger.UpdatedAtUtc = DateTime.UtcNow;
                _context.Update(passenger);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Passengers.Any(e => e.PassengerId == passenger.PassengerId))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Passenger/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var passenger = await _context.Passengers
                .FirstOrDefaultAsync(m => m.PassengerId == id);

            if (passenger == null)
                return NotFound();

            return View(passenger);
        }

        // POST: Passenger/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var passenger = await _context.Passengers.FindAsync(id);

            if (passenger == null)
                return NotFound();

            _context.Passengers.Remove(passenger);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
