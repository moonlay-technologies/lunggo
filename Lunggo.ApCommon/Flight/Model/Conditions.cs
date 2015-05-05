using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    public class TripDetailsConditions : ConditionsBase
    {
        public string BookingId { get; set; }
    }

    public class RevalidateConditions : ConditionsBase
    {
        public string FareId { get; set; }
    }

    public class SpecificSearchConditions : SearchFlightConditions
    {
        public List<FlightSegmentFare> FlightSegments { get; set; }
    }

    public class SearchFlightConditions : ConditionsBase
    {
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public CabinClass CabinClass { get; set; }
    }

    public class ConditionsBase
    {
        public List<FlightTripInfo> TripInfos { get; set; }
    }
}
