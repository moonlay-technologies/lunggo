using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class SearchFlightResult : ResultBase
    {
        public string SearchId { get; set; }
        public List<FlightItineraryFare> FlightItineraries { get; set; }
    }
}
