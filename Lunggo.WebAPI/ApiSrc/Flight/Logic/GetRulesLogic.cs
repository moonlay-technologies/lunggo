using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.Flight.Logic
{
    public  partial class FlightLogic
    {
        public static FlightRulesApiResponse GetRules(FlightRulesApiRequest request)
        {
            if (IsValid(request))
            {
                var service = FlightService.GetInstance();
                var rulesServiceRequest = PreprocessServiceRequest(request);
                var rulesServiceResponse = service.GetRules(rulesServiceRequest);
                var apiResponse = AssembleApiResponse(rulesServiceResponse, request);
                return apiResponse;
            }
            else
            {
                return new FlightRulesApiResponse
                {
                    AirlineRules = new List<AirlineRules>(),
                    BaggageRules = new List<BaggageRules>()
                };
            }
        }

        private static GetRulesInput PreprocessServiceRequest(FlightRulesApiRequest request)
        {
            var service = FlightService.GetInstance();
            return new GetRulesInput
            {
                SearchId = request.SearchId,
                ItinIndex = request.ItinIndex
            };
        }

        private static FlightRulesApiResponse AssembleApiResponse(GetRulesOutput rulesServiceResponse, FlightRulesApiRequest request)
        {
            if (rulesServiceResponse.IsSuccess)
            {
                return new FlightRulesApiResponse
                {
                    AirlineRules = rulesServiceResponse.AirlineRules,
                    BaggageRules = rulesServiceResponse.BaggageRules
                };
            }
            else
            {
                return new FlightRulesApiResponse
                {
                    AirlineRules = null,
                    BaggageRules = null
                };
            }
        }

        private static bool IsValid(FlightRulesApiRequest request)
        {
            return
                request != null &&
                request.SearchId != null;
        }
    }
}