using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class OrderTicketResult : ResultBase
    {
        internal string BookingId { get; set; }
        internal bool IsInstantIssuance { get; set; }
    }
}
