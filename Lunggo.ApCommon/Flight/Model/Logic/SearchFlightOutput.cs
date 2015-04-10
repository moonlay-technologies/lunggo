using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class SearchFlightOutput : OutputBase
    {
        public List<FlightFareItinerary> Itineraries { get; set; }
    }
}
