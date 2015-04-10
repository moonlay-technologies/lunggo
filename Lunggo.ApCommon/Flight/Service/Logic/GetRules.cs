using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Lunggo.ApCommon.Flight.Model;

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
