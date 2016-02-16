using System;
using System.Collections.Generic;
using System.Linq;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightTripForDisplay : FlightTripBase
    {
        public int TotalTransit { get; set; }
        public List<Transit> Transits { get; set; }
        public List<Airline> Airlines { get; set; }
        public double TotalDuration { get; set; }
    }

    public class FlightTrip : FlightTripBase
    {
        
    }

    public class FlightTripBase
    {
        public string OriginAirport { get; set; }
        public string DestinationAirport { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationAirportName { get; set; }
        public string OriginCity { get; set; }
        public string OriginAirportName { get; set; }
        public DateTime DepartureDate { get; set; }
        public List<FlightSegment> Segments { get; set; }

        public bool Identical(FlightTrip otherTrip)
        {
            if (OriginAirport != otherTrip.OriginAirport ||
                DestinationAirport != otherTrip.DestinationAirport ||
                DepartureDate != otherTrip.DepartureDate ||
                Segments == null || 
                otherTrip.Segments == null ||
                Segments.Count != otherTrip.Segments.Count)
                return false;
            for (var i = 0; i< Segments.Count; i++)
            {
                var segment = Segments[i];
                var otherSegment = otherTrip.Segments[i];
                if (segment.DepartureAirport != otherSegment.DepartureAirport ||
                    segment.ArrivalAirport != otherSegment.ArrivalAirport ||
                    segment.DepartureTime != otherSegment.DepartureTime ||
                    segment.ArrivalTime != otherSegment.ArrivalTime ||
                    segment.Duration != otherSegment.Duration ||
                    segment.AirlineCode != otherSegment.AirlineCode ||
                    segment.FlightNumber != otherSegment.FlightNumber ||
                    segment.CabinClass != otherSegment.CabinClass)
                    return false;
            }
            return true;
        }
    }

    public class Airline
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
    }

    public class Transit
    {
        public bool IsStop { get; set; }
        public string Airport { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public TimeSpan Duration { get; set; }
    }

    
}
