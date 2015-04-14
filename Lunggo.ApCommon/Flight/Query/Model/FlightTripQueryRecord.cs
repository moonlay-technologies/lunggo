using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query.Model
{
    internal class FlightTripQueryRecord : QueryRecord
    {
        internal long TripId { get; set; }
        internal long ItineraryId { get; set; }
        internal OriginDestinationInfo Info { get; set; }
    }
}
