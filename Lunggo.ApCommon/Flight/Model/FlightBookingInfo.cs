using System.Collections.Generic;
using Lunggo.ApCommon.ProductBase.Model;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightBookingInfo
    {
        public FlightItinerary Itinerary { get; set; }
        public List<FlightPassenger> Passengers { get; set; }
        public Contact Contact { get; set; }
    }
}
