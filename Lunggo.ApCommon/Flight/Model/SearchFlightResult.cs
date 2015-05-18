using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    internal class SearchFlightResult : ResultBase
    {
        internal string SearchId { get; set; }
        internal List<FlightItineraryFare> FlightItineraries { get; set; }
    }
}
