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
        internal long PassengerId { get; set; }
        internal string RsvNo { get; set; }
        internal PassengerFareInfo Passenger { get; set; }
    }
}
