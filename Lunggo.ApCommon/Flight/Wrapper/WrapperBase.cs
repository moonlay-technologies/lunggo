using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Wrapper
{
    internal abstract class WrapperBase
    {
        internal abstract SearchFlightResult SearchFlight(SearchFlightConditions conditions);
        internal abstract RevalidateFareResult RevalidateFare(RevalidateConditions conditions);
        internal abstract BookFlightResult BookFlight(FlightBookingInfo bookInfo);
        internal abstract OrderTicketResult OrderTicket(string bookingId);
        internal abstract GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions);
        internal abstract List<BookingStatusInfo> GetBookingStatus();
    }
}
