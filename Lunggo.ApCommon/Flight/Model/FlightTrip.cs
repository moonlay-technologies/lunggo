using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightTripApi : FlightTripFare
    {
        public TimeSpan TotalDuration { get; set; }
    }

    public class FlightTripFare : FlightTripBase
    {
        public List<FlightSegmentFare> FlightSegments { get; set; }
    }

    public class FlightTripDetails : FlightTripBase
    {
        public List<FlightSegmentDetails> FlightSegments { get; set; }
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
}
