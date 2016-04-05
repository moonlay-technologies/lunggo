using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class SearchFlightOutput : OutputBase
    {
        public string SearchId { get; set; }
        public List<FlightItineraryForDisplay>[] ItineraryLists { get; set; }
        public List<ComboForDisplay> Combos { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public int Progress { get; set; }
    }
}
