using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightBooking
    {
        public string BookingId { get; set; }
        public bool BookingStatus { get; set; }
        public bool IsTicketOrdered { get; set; }
    }
}
