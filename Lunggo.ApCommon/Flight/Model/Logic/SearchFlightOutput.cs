using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class SearchFlightOutput : OutputBase
    {
        public string SearchId { get; set; }
        public List<FlightItineraryForDisplay> Itineraries { get; set; }
        public List<FlightItineraryForDisplay> ReturnItineraries { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public int TotalSupplier { get; set; }
        public List<int> SearchedSuppliers { get; set; }
    }
}
