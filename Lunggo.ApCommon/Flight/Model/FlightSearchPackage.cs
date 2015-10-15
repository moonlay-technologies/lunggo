using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    internal class FlightSearchPackage
    {
        internal List<FlightItinerary> Itineraries { get; set; }
        internal int Completeness { get; set; }
        internal Dictionary<int, int> CompletenessPointer { get; set; }

        public FlightSearchPackage()
        {
            Itineraries = new List<FlightItinerary>();
            CompletenessPointer = new Dictionary<int, int> {{0, 0}};
        }
    }
}
