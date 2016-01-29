using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
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
                    BaggageRules = new List<BaggageRules>(),
                    OriginalRequest = request
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
                    BaggageRules = rulesServiceResponse.BaggageRules,
                    OriginalRequest = request
                };
            }
            else
            {
                return new FlightRulesApiResponse
                {
                    AirlineRules = null,
                    BaggageRules = null,
                    OriginalRequest = request
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