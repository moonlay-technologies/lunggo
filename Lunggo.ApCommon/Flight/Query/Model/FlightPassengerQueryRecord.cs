using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query.Model
{
    internal class FlightPassengerQueryRecord : QueryRecord
    {
        public long ItineraryId { get; set; }
        public PassengerFareInfo Passenger { get; set; }
    }
}
