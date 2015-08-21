using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    internal class GetTripDetailsResult : ResultBase
    {
        internal string BookingId { get; set; }
        internal int FlightSegmentCount { get; set; }
        internal FlightItinerary FlightItineraries { get; set; }
        internal List<PassengerInfoDetails> Passengers { get; set; }
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
