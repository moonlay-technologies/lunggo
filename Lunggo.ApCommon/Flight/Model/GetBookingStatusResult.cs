using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class GetBookingStatusResult
    {
        public List<BookingStatusInfo> BookingStatusInfos { get; set; }
        public List<string> ChangedScheduleBooking { get; set; }
    }
}
