using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    internal class FlightBooking
    {
        internal string BookingId { get; set; }
        internal BookingStatus BookingStatus { get; set; }
        internal bool IsTicketOrdered { get; set; }
    }
}
