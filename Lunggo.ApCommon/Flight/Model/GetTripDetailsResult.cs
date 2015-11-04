using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model
{
    internal class GetTripDetailsResult : ResultBase
    {
        internal string BookingId { get; set; }
        internal int FlightSegmentCount { get; set; }
        internal FlightItinerary Itinerary { get; set; }
        internal List<FlightPassenger> Passengers { get; set; }
        internal decimal TotalFare { get; set; }
        internal decimal AdultTotalFare { get; set; }
        internal decimal ChildTotalFare { get; set; }
        internal decimal InfantTotalFare { get; set; }
        internal string Currency { get; set; }
        internal List<string> BookingNotes { get; set; }

        internal GetTripDetailsResult()
        {
            BookingNotes = new List<string>();
        }
    }
}
