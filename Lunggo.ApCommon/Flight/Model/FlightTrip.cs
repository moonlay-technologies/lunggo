using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightTripApi : FlightTripBase
    {
        public int TotalTransit { get; set; }
        public List<Transit> Transits { get; set; }
        public List<Airline> Airlines { get; set; }
        public List<FlightSegmentApi> FlightSegments { get; set; }
        public string OriginCity { get; set; }
        public string OriginAirportName { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationAirportName { get; set; }
        public TimeSpan TotalDuration { get; set; }
    }

    public class FlightTripFare : FlightTripBase
    {
        public List<FlightSegmentFare> FlightSegments { get; set; }
    }

    public class FlightTripDetails : FlightTripBase
    {
        public List<FlightSegmentDetails> FlightSegments { get; set; }
        public string OriginCity { get; set; }
        public string OriginAirportName { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationAirportName { get; set; }
    }

    public class FlightTripInfo : FlightTripBase
    {
        
    }

    public class FlightTripBase
    {
        public string OriginAirport { get; set; }
        public string DestinationAirport { get; set; }
        public DateTime DepartureDate { get; set; }
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
