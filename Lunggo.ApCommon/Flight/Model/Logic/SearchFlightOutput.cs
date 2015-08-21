using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class SearchFlightOutput : OutputBase
    {
        public string SearchId { get; set; }
        public List<FlightItinerary> Itineraries { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
