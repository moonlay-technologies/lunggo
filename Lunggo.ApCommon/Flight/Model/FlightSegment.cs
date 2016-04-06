using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightSegment
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
        public string DepartureTerminal { get; set; }
        public string DepartureCity { get; set; }
        public string DepartureAirportName { get; set; }
        public string ArrivalTerminal { get; set; }
        public string ArrivalCity { get; set; }
        public string ArrivalAirportName { get; set; }
        public string AirlineName { get; set; }
        public string AirlineLogoUrl { get; set; }
        public string OperatingAirlineName { get; set; }
        public string OperatingAirlineLogoUrl { get; set; }
        public List<FlightStop> Stops { get; set; }
        public CabinClass CabinClass { get; set; }
        public bool Meal { get; set; }
        public string Baggage { get; set; }
        public string Pnr { get; set; }
        public int RemainingSeats { get; set; }

        public bool Identical(FlightSegment otherSegment)
        {
            return
                DepartureAirport == otherSegment.DepartureAirport &&
                ArrivalAirport == otherSegment.ArrivalAirport &&
                DepartureTime == otherSegment.DepartureTime &&
                ArrivalTime == otherSegment.ArrivalTime &&
                Duration == otherSegment.Duration &&
                AirlineCode == otherSegment.AirlineCode &&
                FlightNumber == otherSegment.FlightNumber &&
                CabinClass == otherSegment.CabinClass;
        }
    }
    public class FlightStop
    {
        public string Airport { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}