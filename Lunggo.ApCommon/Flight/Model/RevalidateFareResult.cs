using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class RevalidateFareResult : ResultBase
    {
        public bool IsValid { get; set; }
        public FlightItineraryFare Itinerary { get; set; }
    }
}
