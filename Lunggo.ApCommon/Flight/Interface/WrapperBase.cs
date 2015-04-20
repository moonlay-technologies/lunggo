using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Interface
{
    internal abstract class WrapperBase
    {
        internal abstract SearchFlightResult SearchFlight(SearchFlightConditions conditions);
        internal abstract SearchFlightResult SpecificSearchFlight(SpecificSearchConditions flightFareItinerary);
        internal abstract RevalidateFareResult RevalidateFare(RevalidateConditions conditions);
        internal abstract BookFlightResult BookFlight(FlightBookingInfo bookInfo);
        internal abstract OrderTicketResult OrderTicket(string bookingId);
        internal abstract GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions);
        internal abstract CancelBookingResult CancelBooking(string bookingId);
        internal abstract GetBookingStatusResult GetBookingStatus();
        internal abstract GetRulesResult GetRules(string fareId);
    }
}
