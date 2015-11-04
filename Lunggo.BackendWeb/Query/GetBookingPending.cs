using Lunggo.Framework.Database;

namespace Lunggo.BackendWeb.Query
{
    public class GetBookingPending : QueryBase<GetBookingPending, GetBookingPendingRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            return "SELECT RsvNo, ContactName, RsvTime, RsvStatusCd, FinalPrice, PaymentMethodCd, PaymentStatusCd, 'Hotel' AS Type  FROM HotelReservations WHERE PaymentStatusCd = 01 UNION ALL SELECT RsvNo, ContactName, RsvTime, RsvStatusCd, FinalPrice, PaymentMethodCd, PaymentStatusCd, 'Flight' AS Type FROM FlightReservations WHERE PaymentStatusCd = 01";
        }

    }
}