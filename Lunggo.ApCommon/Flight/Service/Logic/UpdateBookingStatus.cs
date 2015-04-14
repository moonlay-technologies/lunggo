using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public void UpdateBookingStatus()
        {
            var result = GetBookingStatusInternal();
            var query = UpdateBookingStatusQuery.GetInstance();
            // TODO FLIGHT : Schedule change Notification
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                query.Execute(conn, result.BookingStatusInfos);
            }
        }
    }
}
