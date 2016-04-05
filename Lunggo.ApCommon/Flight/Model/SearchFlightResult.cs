using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model
{
    public class SearchFlightResult : ResultBase
    {
        internal List<FlightItinerary> Itineraries { get; set; }
    }
}
