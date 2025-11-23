using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Aeromvp.Models;
using Aeromvp.ViewModels;

namespace Aeromvp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // ⚠️ IMPORTANTE:
            // La vista Index.cshtml EXIGE un modelo FlightResultViewModel
            // así que debemos enviarlo, aunque sea vacío.

            var model = new FlightResultViewModel
            {
                OriginCity = "",
                OriginCode = "",
                DestinationCity = "",
                DestinationCode = "",
                DepartureDate = DateTime.Now.Date,
                ReturnDate = null,
                Adults = 1,
                Children = 0,
                Infants = 0,
                Flights = new List<FlightResultViewModel.FlightCard>(),
                CurrentPage = 1,
                TotalPages = 1
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
