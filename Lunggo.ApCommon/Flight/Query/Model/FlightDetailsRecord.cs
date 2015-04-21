using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Query.Model
{
    internal class FlightDetailsRecord
    {
        internal string BookingId { get; set; }
        internal List<FlightSegmentDetails> Segments { get; set; }
        internal List<PassengerInfoDetails> Passengers { get; set; }
    }

    internal class FlightDetailsSegmentRecord
    {
        internal string BookingId { get; set; }
        internal BookingStatus BookingStatus { get; set; }
        internal List<long> FlightSegmentPrimKeys { get; set; }
        internal string Pnr { get; set; }
        internal string DepartureTerminal { get; set; }
        internal string ArrivalTerminal { get; set; }
        internal string Baggage { get; set; }
        public DateTime DepartureTime { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
    }
}
