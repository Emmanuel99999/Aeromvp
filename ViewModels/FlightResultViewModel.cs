using System;
using System.Collections.Generic;

namespace Aeromvp.ViewModels
{
    public class FlightResultViewModel
    {
        // ------------------------------
        // Información general de búsqueda
        // ------------------------------
        public string OriginCity { get; set; } = null!;
        public string OriginCode { get; set; } = null!;
        public string DestinationCity { get; set; } = null!;
        public string DestinationCode { get; set; } = null!;

        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public int Adults { get; set; }
        public int Children { get; set; }
        public int Infants { get; set; }

        // ------------------------------
        // Resultados de vuelos (Cards)
        // ------------------------------
        public List<FlightCard> Flights { get; set; } = new();

        // ------------------------------
        // Paginación
        // ------------------------------
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        // ==============================
        // SUBCLASE: CARD DE RESULTADO
        // ==============================
        public class FlightCard
        {
            public int FlightId { get; set; }

            public string AirlineName { get; set; } = null!;
            public string AirlineCode { get; set; } = null!;

            public string FlightNumber { get; set; } = null!;
            public string CabinClass { get; set; } = null!;

            public string OriginCode { get; set; } = null!;
            public string DestinationCode { get; set; } = null!;

            public DateTime DepartureTimeUtc { get; set; }
            public DateTime ArrivalTimeUtc { get; set; }

            public string DurationText { get; set; } = null!;
            public string StopsText { get; set; } = null!;

            public decimal Price { get; set; }

            // Para botones como “Detalles”
            public bool HasDetails { get; set; } = true;
        }
    }
}
