using System.Collections.Generic;
using System.Linq;
using FluentSharp.CoreLib;
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
        public void GetAndUpdateBookingStatus(out List<string> ticketedBookingIds, out List<string> scheduleChangedBookingIds)
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
                        BookingStatus = BookingStatusCd.Mnemonic(info.BookingStatus)
                    }).ToArray();
                    query.Execute(conn, dbBookingStatusInfo);
                }
            }
            ticketedBookingIds = statusData.Where(data => data.BookingStatus == BookingStatus.Ticketed).Select(data => data.BookingId).toList();
            scheduleChangedBookingIds = statusData.Where(data => data.BookingStatus == BookingStatus.ScheduleChanged).Select(data => data.BookingId).toList();
        }
    }
}
