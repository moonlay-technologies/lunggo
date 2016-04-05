using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class RevalidateFlightOutput : OutputBase
    {
        public bool IsValid { get; set; }
        public bool IsItineraryChanged { get; set; }
        public bool IsPriceChanged { get; set; }
        public FlightItineraryForDisplay NewItinerary { get; set; }
        public decimal? NewPrice { get; set; }
        public string Token { get; set; }
    }

    public class RevalidateFlightOutputSet
    {
        public bool IsSuccess { get; set; }
        public bool IsValid { get; set; }
        public bool IsItineraryChanged { get; set; }
        public bool IsPriceChanged { get; set; }
        public FlightItinerary NewItinerary { get; set; }
    }
}
