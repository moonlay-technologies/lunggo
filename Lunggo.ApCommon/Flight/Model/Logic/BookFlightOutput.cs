using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class BookFlightOutput : OutputBase
    {
        public bool IsValid { get; set; }
        public bool IsItineraryChanged { get; set; }
        public bool IsPriceChanged { get; set; }
        public FlightItineraryForDisplay NewItinerary { get; set; }
        public decimal? NewPrice { get; set; }
        public string RsvNo { get; set; }
        public DateTime TimeLimit { get; set; }
        public bool Test { get; set; }
    }

    public class BookResult
    {
        public bool IsSuccess { get; set; }
        public RevalidateFlightOutputSet RevalidateSet { get; set; }
        public DateTime TimeLimit { get; set; }
    }
}
