using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightSearchPackage
    {
        public List<FlightItinerary> Itineraries { get; set; }
        public int Completeness { get; set; }
        public Dictionary<int, int> CompletenessPointer { get; set; }

        public FlightSearchPackage()
        {
            Itineraries = new List<FlightItinerary>();
            CompletenessPointer = new Dictionary<int, int>();
        }
    }
}
