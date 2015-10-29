using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class SearchFlightResult : ResultBase
    {
        internal string SearchId { get; set; }
        internal List<FlightItinerary> Itineraries { get; set; }
    }
}
