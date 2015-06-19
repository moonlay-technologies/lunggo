using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public void GetAndUpdateBookingStatus(out List<string> ticketedRsvNos, out List<string> scheduleChangedRsvNos)
        {
            var statusData = GetBookingStatusInternal();
            if (statusData.Any())
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var query = UpdateFlightBookingStatusQuery.GetInstance();
                    var dbBookingStatusInfo = statusData.Select(info => new
                    {
                        info.BookingId,
                        BookingStatusCd = BookingStatusCd.Mnemonic(info.BookingStatus)
                    }).ToArray();
                    query.Execute(conn, dbBookingStatusInfo);
                }
            }
            var ticketedBookingIds = statusData.Where(data => data.BookingStatus == BookingStatus.Ticketed).Select(data => data.BookingId).ToList();
            var scheduleChangedBookingIds = statusData.Where(data => data.BookingStatus == BookingStatus.ScheduleChanged).Select(data => data.BookingId).ToList();
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                ticketedRsvNos = ticketedBookingIds.Any()
                    ? GetFlightRsvNoQuery.GetInstance().Execute(conn, ticketedBookingIds).Distinct().ToList()
                    : new List<string>();
                scheduleChangedRsvNos = scheduleChangedBookingIds.Any()
                    ? GetFlightRsvNoQuery.GetInstance().Execute(conn, scheduleChangedBookingIds).Distinct().ToList()
                    : new List<string>();
            }
        }
    }
}
