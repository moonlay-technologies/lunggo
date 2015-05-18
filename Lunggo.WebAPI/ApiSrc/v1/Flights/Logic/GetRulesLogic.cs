using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightRulesApiResponse GetRules(FlightRulesApiRequest request)
        {
            if (IsValid(request))
            {
                var service = FlightService.GetInstance();
                if (request.HashKey == null)
                    request.HashKey = service.SaveItineraryToCache(request.SearchId, request.ItinIndex);
                var rulesServiceRequest = PreprocessServiceRequest(request);
                var rulesServiceResponse = service.GetRules(rulesServiceRequest);
                var apiResponse = AssembleApiResponse(rulesServiceResponse, request);
                return apiResponse;
            }
            else
            {
                return new FlightRulesApiResponse
                {
                    HashKey = null,
                    AirlineRules = new List<AirlineRules>(),
                    BaggageRules = new List<BaggageRules>(),
                    OriginalRequest = request
                };
            }
        }

        private static GetRulesInput PreprocessServiceRequest(FlightRulesApiRequest request)
        {
            var service = FlightService.GetInstance();
            var itin = service.GetItineraryFromCache(request.HashKey);
            return new GetRulesInput { FareId = itin.FareId };
        }

        private static FlightRulesApiResponse AssembleApiResponse(GetRulesOutput rulesServiceResponse, FlightRulesApiRequest request)
        {
            if (rulesServiceResponse.IsSuccess)
            {
                return new FlightRulesApiResponse
                {
                    HashKey = request.HashKey,
                    AirlineRules = rulesServiceResponse.AirlineRules,
                    BaggageRules = rulesServiceResponse.BaggageRules,
                    OriginalRequest = request
                };
            }
            else
            {
                return new FlightRulesApiResponse
                {
                    HashKey = null,
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
                (request.SearchId != null || request.HashKey != null);
        }
    }
}