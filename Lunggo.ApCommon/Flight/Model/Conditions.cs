using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    public class TripDetailsConditions : ConditionsBase
    {
        public string BookingId { get; set; }
        public Supplier Supplier { get; set; }
    }

    public class RevalidateConditions : ConditionsBase
    {
        public FlightItinerary Itinerary { get; set; }
    }

    public class SearchFlightConditions : ConditionsBase
    {
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public CabinClass CabinClass { get; set; }
        public List<string> AirlinePreferences { get; set; }
        public List<string> AirlineExcludes { get; set; }
    }

    public class ConditionsBase
    {
        public List<FlightTrip> Trips { get; set; }
    }
}
