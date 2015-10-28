using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public GetRulesOutput GetRules(GetRulesInput input)
        {
            var itin = GetItineraryFromSearchCache(input.SearchId, "0", input.ItinIndex);
            var rules = GetRulesInternal(itin.FareId);
            var output = new GetRulesOutput
            {
                IsSuccess = rules.IsSuccess,
                Errors = rules.Errors,
                ErrorMessages = rules.ErrorMessages
            };
            if (rules.IsSuccess)
            {
                output.AirlineRules = rules.AirlineRules;
                output.BaggageRules = rules.BaggageRules;
            }
            return output;
        }
    }
}
