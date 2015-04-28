using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class SearchFlightOutput : OutputBase
    {
        public string SearchId { get; set; }
        public List<FlightItineraryFare> Itineraries { get; set; }
    }
}
