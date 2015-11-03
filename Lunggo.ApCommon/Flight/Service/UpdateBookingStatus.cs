using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public void GetAndUpdateBookingStatus(out List<string> ticketedRsvNos, out List<string> scheduleChangedRsvNos)
        {
            var statusData = GetBookingStatusInternal();
            if (statusData.Any())
                UpdateDb.BookingStatus(statusData);
            var ticketedBookingIds = statusData.Where(data => data.BookingStatus == BookingStatus.Ticketed).Select(data => data.BookingId).ToList();
            var scheduleChangedBookingIds = statusData.Where(data => data.BookingStatus == BookingStatus.ScheduleChanged).Select(data => data.BookingId).ToList();
            var rsvNosWithTicketedBooking = ticketedBookingIds.Any()
                ? GetDb.RsvNoByBookingId(ticketedBookingIds).Distinct()
                : new List<string>();
            var rsvsWithTicketedBooking = rsvNosWithTicketedBooking.Select(GetReservation);
            ticketedRsvNos =
                rsvsWithTicketedBooking.Where(
                    rsv =>
                        rsv.Itineraries.TrueForAll(
                            itin =>
                                itin.BookingStatus == BookingStatus.Ticketed ||
                                itin.BookingStatus == BookingStatus.ScheduleChanged))
                    .Select(rsv => rsv.RsvNo).ToList();
            scheduleChangedRsvNos = scheduleChangedBookingIds.Any()
                ? GetDb.RsvNoByBookingId(scheduleChangedBookingIds).Distinct().ToList()
                : new List<string>();
        }
    }
}
