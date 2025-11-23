using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aeromvp.Data;
using Aeromvp.Models;

namespace Aeromvp.Controllers
{
    public class FlightController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Flight/Search
        public IActionResult Search()
        {
            return View();
        }

        // POST: /Flight/SearchResults
        [HttpPost]
        public async Task<IActionResult> SearchResults(string origin, string destination, DateTime date)
        {
            if (string.IsNullOrWhiteSpace(origin) || string.IsNullOrWhiteSpace(destination))
                return BadRequest("Origin and destination are required.");

            var flights = await _context.Flights
                .Where(f =>
                    f.OriginAirportCode == origin &&
                    f.DestinationAirportCode == destination &&
                    f.DepartureTimeUtc.Date == date.Date &&
                    f.Status == "Scheduled")
                .AsNoTracking()
                .ToListAsync();

            return View(flights);
        }

        // GET: /Flight/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var flight = await _context.Flights
                .Include(f => f.Fares)
                .Include(f => f.Legs)
                .FirstOrDefaultAsync(f => f.FlightId == id);

            if (flight == null)
                return NotFound();

            return View(flight);
        }
    }
}
