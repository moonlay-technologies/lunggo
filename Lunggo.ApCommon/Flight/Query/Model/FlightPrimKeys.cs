using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Query.Model
{
    internal class FlightPrimKeys
    {
        internal string RsvNo { get; set; }
        internal List<long> ItineraryId { get; set; }
        internal List<long> TripId { get; set; }
        internal List<long> SegmentId { get; set; }
        internal List<long> StopId { get; set; }
        internal List<long> PassengerId { get; set; }
    }
}
