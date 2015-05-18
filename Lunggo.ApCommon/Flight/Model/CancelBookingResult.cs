using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    internal class CancelBookingResult : ResultBase
    {
        internal string BookingId { get; set; }
    }
}
