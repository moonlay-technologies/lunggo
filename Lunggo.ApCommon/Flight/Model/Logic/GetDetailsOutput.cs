using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class GetDetailsOutput : OutputBase
    {
        public List<DetailsResult> DetailsResults { get; set; }

        public GetDetailsOutput()
        {
            DetailsResults = new List<DetailsResult>();
        }
    }

    public class DetailsResult
    {
        public bool IsSuccess { get; set; }
        public string BookingId { get; set; }
        public int SegmentCount { get; set; }
        public FlightItinerary Itinerary { get; set; }
        public decimal TotalFare { get; set; }
        public decimal AdultTotalFare { get; set; }
        public decimal ChildTotalFare { get; set; }
        public decimal InfantTotalFare { get; set; }
        public string Currency { get; set; }
        public List<string> BookingNotes { get; set; }
    }
}
