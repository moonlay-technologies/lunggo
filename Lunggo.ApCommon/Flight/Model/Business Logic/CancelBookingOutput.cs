using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class CancelBookingOutput
    {
        public bool IsCancelSuccess { get; set; }
        public string BookingId { get; set; }
        public bool ReturnIsCancelSuccess { get; set; }
        public string ReturnBookingId { get; set; }
    }
}
