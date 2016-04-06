using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightBookingInfo
    {
        public FlightItinerary Itinerary { get; set; }
        public List<FlightPassenger> Passengers { get; set; }
        public Contact Contact { get; set; }
    }
}
