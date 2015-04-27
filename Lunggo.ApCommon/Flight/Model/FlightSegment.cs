using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightSegmentFare : FlightSegmentBase
    {
        public string CabinClass { get; set; }
        public int RemainingSeats { get; set; }
        public List<FlightStop> FlightStops { get; set; }
    }

    public class FlightSegmentDetails : FlightSegmentBase
    {
        public int Reference { get; set; }
        public string Pnr { get; set; }
        public string DepartureTerminal { get; set; }
        public string ArrivalTerminal { get; set; }
        public string Baggage { get; set; }
    }

    public class FlightSegmentBase
    {
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public TimeSpan Duration { get; set; }
        public int StopQuantity { get; set; }
        public string AirlineCode { get; set; }
        public string FlightNumber { get; set; }
        public string OperatingAirlineCode { get; set; }
        public string AircraftCode { get; set; }
        public string Rbd { get; set; }
    }

    public class FlightStop
    {
        public string Airport { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}