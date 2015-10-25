using System.Collections.Generic;
using System.ServiceModel.Security.Tokens;
using CsQuery.EquationParser.Implementation.Functions;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Wrapper
{
    internal abstract class FlightSupplierWrapperBase
    {
        internal abstract Supplier SupplierName { get; }

        internal abstract void Init();
        internal abstract SearchFlightResult SearchFlight(SearchFlightConditions conditions);
        internal abstract RevalidateFareResult RevalidateFare(RevalidateConditions conditions);
        internal abstract BookFlightResult BookFlight(FlightBookingInfo bookInfo);
        internal abstract OrderTicketResult OrderTicket(string bookingId, bool canHold);
        internal abstract GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions);
        internal abstract List<BookingStatusInfo> GetBookingStatus();
    }
}
