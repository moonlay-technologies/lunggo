using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    public class IssueTicketInfo
    {
        public string BookingId { get; set; }
        public bool CanHold { get; set; }
        public Supplier Supplier { get; set; }
    }
}
