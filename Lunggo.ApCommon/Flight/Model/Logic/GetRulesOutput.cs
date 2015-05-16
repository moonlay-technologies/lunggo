using System.Collections.Generic;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class GetRulesOutput : OutputBase
    {
        public List<AirlineRules> AirlineRules { get; set; }
        public List<BaggageRules> BaggageRules { get; set; }
    }
}