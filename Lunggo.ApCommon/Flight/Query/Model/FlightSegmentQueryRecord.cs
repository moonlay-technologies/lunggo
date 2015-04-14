using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query.Model
{
    internal class FlightSegmentQueryRecord : QueryRecord
    {
        internal long SegmentId { get; set; }
        internal long TripId { get; set; }
        internal FlightFareTrip Segment { get; set; }
    }
}
