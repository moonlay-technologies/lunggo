using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model
{
    public class GetRulesOutput : OutputBase
    {
        public FlightRules Rules { get; set; }
    }

    public class FlightRules
    {
        public List<AirlineRules> AirlineRules { get; set; }
        public List<BaggageRules> BaggageRules { get; set; }
    }
}