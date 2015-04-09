using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class IssueTicketInput
    {
        public string BookingId { get; set; }
        public string ReturnBookingId { get; set; }
        public ReservationDetails ReservationDetails { get; set; }
    }
}
