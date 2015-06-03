using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class GetDetailsOutput : OutputBase
    {
        public string BookingId { get; set; }
        public int FlightSegmentCount { get; set; }
        public FlightItineraryDetails FlightItineraryDetails { get; set; }
        public decimal TotalFare { get; set; }
        public decimal AdultTotalFare { get; set; }
        public decimal ChildTotalFare { get; set; }
        public decimal InfantTotalFare { get; set; }
        public string Currency { get; set; }
        public List<string> BookingNotes { get; set; }
    }
}
