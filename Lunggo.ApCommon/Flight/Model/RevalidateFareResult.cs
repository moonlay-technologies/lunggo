using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class RevalidateFareResult : ResultBase
    {
        public FlightFareItinerary FlightFareItinerary { get; set; }
        public bool IsValid { get; set; }
        public bool IsHigherFareAvailable { get; set; }
        public bool IsCabinTypeChanged { get; set; }
        public bool IsRBDChanged { get; set; }
    }
}
