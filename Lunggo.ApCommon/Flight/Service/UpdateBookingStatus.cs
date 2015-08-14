using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Logic;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Database;
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
                UpdateFlightDb.UpdateBookingStatus(statusData);
            var ticketedBookingIds = statusData.Where(data => data.BookingStatus == BookingStatus.Ticketed).Select(data => data.BookingId).ToList();
            var scheduleChangedBookingIds = statusData.Where(data => data.BookingStatus == BookingStatus.ScheduleChanged).Select(data => data.BookingId).ToList();
            ticketedRsvNos = ticketedBookingIds.Any()
                ? GetFlightDb.RsvNoByBookingId(ticketedBookingIds)
                : new List<string>();
            scheduleChangedRsvNos = scheduleChangedBookingIds.Any()
                ? GetFlightDb.RsvNoByBookingId(scheduleChangedBookingIds)
                : new List<string>();
        }
    }
}
