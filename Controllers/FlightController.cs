using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Aeromvp.Data;
using Aeromvp.Models;
using Aeromvp.ViewModels;

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
        [HttpGet]
        public IActionResult Search()
        {
            var defaultSearch = new FlightSearchInputViewModel
            {
                DepartureDate = DateTime.Today,
                Adults = 1,
                Children = 0,
                Infants = 0
            };

            return View(defaultSearch);
        }

        // POST: /Flight/SearchResults
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchResults(FlightSearchInputViewModel search)
        {
            if (!ModelState.IsValid)
            {
                return View("Search", search);
            }

            var originCode = search.OriginCode?.Trim().ToUpperInvariant() ?? string.Empty;
            var destinationCode = search.DestinationCode?.Trim().ToUpperInvariant() ?? string.Empty;

            var flights = await _context.Flights
                .Include(f => f.Fares)
                .Include(f => f.Legs)
                .Where(f =>
                    f.Status == "Scheduled" &&
                    f.OriginAirportCode.ToUpper() == originCode &&
                    f.DestinationAirportCode.ToUpper() == destinationCode &&
                    f.DepartureTimeUtc.Date == search.DepartureDate.Date)
                .AsNoTracking()
                .ToListAsync();

            var viewModel = new FlightResultViewModel
            {
                OriginCity = originCode,
                OriginCode = originCode,
                DestinationCity = destinationCode,
                DestinationCode = destinationCode,
                DepartureDate = search.DepartureDate.Date,
                ReturnDate = search.ReturnDate,
                Adults = search.Adults,
                Children = search.Children,
                Infants = search.Infants,
                Flights = flights.Select(MapToCard).ToList(),
                CurrentPage = 1,
                TotalPages = 1
            };

            return View("~/Views/Home/Index.cshtml", viewModel);
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

        private FlightResultViewModel.FlightCard MapToCard(Flight flight)
        {
            var cheapestFare = flight.Fares
                .OrderBy(f => f.TotalPrice == 0 ? f.BaseFare + f.Taxes + f.AdditionalFees : f.TotalPrice)
                .FirstOrDefault();

            var price = cheapestFare?.TotalPrice > 0
                ? cheapestFare!.TotalPrice
                : (cheapestFare?.BaseFare ?? 0) + (cheapestFare?.Taxes ?? 0) + (cheapestFare?.AdditionalFees ?? 0);

            return new FlightResultViewModel.FlightCard
            {
                FlightId = flight.FlightId,
                AirlineName = flight.MarketingAirline ?? flight.OperatingAirline ?? "Aerolínea",
                AirlineCode = flight.MarketingAirline ?? flight.OperatingAirline ?? "-",
                FlightNumber = flight.FlightNumber,
                CabinClass = cheapestFare?.CabinClass ?? "Económica",
                OriginCode = flight.OriginAirportCode,
                DestinationCode = flight.DestinationAirportCode,
                DepartureTimeUtc = flight.DepartureTimeUtc,
                ArrivalTimeUtc = flight.ArrivalTimeUtc,
                DurationText = FormatDuration(flight.DurationMinutes),
                StopsText = FormatStops(flight.Legs?.Count ?? 0),
                Price = price,
                HasDetails = true
            };
        }

        private string FormatDuration(int totalMinutes)
        {
            var duration = TimeSpan.FromMinutes(totalMinutes);
            return $"{(int)duration.TotalHours}h {duration.Minutes:D2}m";
        }

        private string FormatStops(int legsCount)
        {
            if (legsCount <= 1) return "Directo";
            return legsCount == 2 ? "1 escala" : $"{legsCount - 1} escalas";
        }
    }
}
