using System.Collections.Generic;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightBookingInfo
    {
        public FlightItinerary Itinerary { get; set; }
        public List<Pax> Passengers { get; set; }
        public Contact Contact { get; set; }
        public bool Test { get; set; }
    }
}
