using Lunggo.ApCommon.Flight.Query;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public void UpdateBookingStatus()
        {
            var result = GetBookingStatusInternal();
            // TODO FLIGHT : Schedule change Notification
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                UpdateBookingStatusQuery.GetInstance().Execute(conn, result.BookingStatusInfos);
            }
        }
    }
}
