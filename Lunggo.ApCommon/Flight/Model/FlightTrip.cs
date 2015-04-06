using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightFareTrip : FlightTripBase
    {
        public string CabinClass { get; set; }
        public int RemainingSeats { get; set; }
    }

    public class FlightTripDetails : FlightTripBase
    {
        public string DepartureTerminal { get; set; }
        public string ArrivalTerminal { get; set; }
        public string Baggage { get; set; }
        public int StopQuantity { get; set; }
    }

    public class FlightTripBase
    {
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public int Duration { get; set; }
        public string AirlineCode { get; set; }
        public string FlightNumber { get; set; }
        public string AircraftCode { get; set; }
        public string RBD { get; set; }
    }
}