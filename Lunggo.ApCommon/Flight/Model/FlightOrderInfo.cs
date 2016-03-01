using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model
{
    internal class FlightOrderInfo
    {
        internal string BookingId { get; set; }
        internal List<FlightPassenger> Passengers { get; set; }
        internal Contact Contact { get; set; }
    }
}
