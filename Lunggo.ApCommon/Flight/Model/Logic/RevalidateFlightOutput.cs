using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class RevalidateFlightOutput : OutputBase
    {
        public List<RevalidateFlightOutputSet> Sets { get; set; }
        public bool IsValid { get; set; }
        public decimal? NewFare { get; set; }
        public string Token { get; set; }

        public RevalidateFlightOutput()
        {
            Sets = new List<RevalidateFlightOutputSet>();
        }
    }

    public class RevalidateFlightOutputSet
    {
        public bool IsSuccess { get; set; }
        public bool IsValid { get; set; }
        public FlightItinerary Itinerary { get; set; }
    }
}
