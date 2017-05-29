using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket
{
    internal partial class TiketWrapper
    {
        internal override BookFlightResult BookFlight(FlightBookingInfo bookInfo)
        {
            return new BookFlightResult();
        }

        internal override RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
        {
            return new RevalidateFareResult();
        }

        internal override IssueTicketResult OrderTicket(string bookingId, bool canHold)
        {
            return new IssueTicketResult();
        }

        internal override GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions)
        {
            return new GetTripDetailsResult();
        }

        internal override Currency CurrencyGetter(string currency)
        {
            return new Currency(currency, Supplier.Sriwijaya);
        }

        internal override decimal GetDeposit()
        {
            return 0;
        }

        internal override List<BookingStatusInfo> GetBookingStatus()
        {
            throw new NotImplementedException();
        }
    }
}
