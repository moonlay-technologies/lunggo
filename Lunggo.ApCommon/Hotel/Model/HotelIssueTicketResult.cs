using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelIssueTicketResult
    {
        internal List<string> BookingId { get; set; }
        internal string RsvNo { get; set; }
        internal string Status { get; set; }
        internal bool IsInstantIssuance { get; set; }
        internal bool IsSuccess { get; set; }
       
    }
}
