using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelIssueTicketResult
    {
        internal string BookingId { get; set; }
        internal string RsvNo { get; set; }
        internal string Status { get; set; }
        internal bool IsInstantIssuance { get; set; }
    }
}
