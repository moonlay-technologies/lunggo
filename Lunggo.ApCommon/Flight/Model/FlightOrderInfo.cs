using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    internal class FlightOrderInfo
    {
        internal string BookingId { get; set; }
        internal List<PassengerInfoFare> PassengerInfoFares { get; set; }
        internal ContactData ContactData { get; set; }
    }
}
