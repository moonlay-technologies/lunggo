using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public GetRulesOutput GetRules(GetRulesInput input)
        {
            var output = new GetRulesOutput();
            var rules = GetRulesInternal(input.FareId);
            output.Rules = new FlightRules();
            if (rules.IsSuccess)
            {
                output.Rules.AirlineRules = rules.AirlineRules;
                output.Rules.BaggageRules = rules.BaggageRules;
            }
            return output;
        }
    }
}
