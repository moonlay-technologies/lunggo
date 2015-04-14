using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query.Model
{
    internal class FlightStopQueryRecord : QueryRecord
    {
        internal long SegmentId { get; set; }
        internal FlightStop Stop { get; set; }
    }
}
