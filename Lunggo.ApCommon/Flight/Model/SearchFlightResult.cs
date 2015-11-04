using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model
{
    public class SearchFlightResult : ResultBase
    {
        internal string SearchId { get; set; }
        internal List<FlightItinerary> Itineraries { get; set; }
    }
}
