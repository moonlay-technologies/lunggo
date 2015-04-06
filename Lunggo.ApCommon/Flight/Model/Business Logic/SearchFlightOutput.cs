using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class SearchFlightOutput
    {
        public bool IsReturnType { get; set; }
        public bool Any { get; set; }
        public bool ReturnAny { get; set; }
        public List<FlightFareItinerary> Itineraries { get; set; }
        public List<FlightFareItinerary> ReturnItineraries { get; set; }
    }
}
