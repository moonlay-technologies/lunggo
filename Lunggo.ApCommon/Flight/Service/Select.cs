using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public string SelectFlight(string searchId, int itinIndex)
        {
            var token = SaveItineraryFromSearchToCache(searchId, itinIndex);
            return token;
        }
    }
}
