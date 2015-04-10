using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override GetRulesResult GetRules(string fareId)
        {
            using (var client = new MystiflyClientHandler())
            {
                var request = new AirRulesRQ1
                {
                    FareSourceCode = fareId,
                    SessionId = client.SessionId,
                    Target = MystiflyClientHandler.Target,
                    ExtensionData = null
                };
                var result = new GetRulesResult();
                var retry = 0;
                var done = false;
                while (!done)
                {
                    var response = client.FareRules1_1(request);
                    done = true;
                    if (!response.Errors.Any() && response.Success)
                    {
                        result = MapResult(response);
                        result.IsSuccess = true;
                    }
                    else
                    {
                        if (response.Errors.Any())
                        {
                            result.Errors = new List<FlightError>();
                            result.ErrorMessages = new List<string>();
                            if (response.Errors.Length == 1 && response.Errors.Single().Code == "ERFRU012")
                            {
                                result.Errors = null;
                                result.ErrorMessages = null;
                                result.IsSuccess = true;
                                result.AirlineRules = null;
                                result.BaggageRules = null;
                            }
                            else
                            {

                                foreach (var error in response.Errors)
                                {
                                    if (error.Code == "ERFRU013")
                                    {
                                        result.Errors = null;
                                        result.ErrorMessages = null;
                                        client.CreateSession();
                                        request.SessionId = client.SessionId;
                                        retry++;
                                        if (retry <= 3)
                                        {
                                            done = false;
                                            break;
                                        }
                                    }
                                    MapError(response, result);
                                }
                            }
                        }
                        result.IsSuccess = false;
                    }
                }
                return result;
            }
        }

        private static GetRulesResult MapResult(AirRulesRS1 response)
        {
            return new GetRulesResult
            {
                AirlineRules = response.FareRules.Select(fareRule => new AirlineRules
                {
                    AirlineCode = fareRule.Airline,
                    DepartureAirport = fareRule.CityPair.Substring(0, 3),
                    ArrivalAirport = fareRule.CityPair.Substring(3, 3),
                    Rules = fareRule.RuleDetails.Select(rule => rule.Rules).ToList()
                }).ToList(),
                BaggageRules = response.BaggageInfos.Select(baggageRule => new BaggageRules
                {
                    AirlineCode = baggageRule.FlightNo.Substring(0, 2),
                    FlightNumber = baggageRule.FlightNo.Substring(2),
                    DepartureAirport = baggageRule.Departure,
                    ArrivalAirport = baggageRule.Arrival,
                    Baggage = baggageRule.Baggage
                }).ToList()
            };
        }

        private static void MapError(AirRulesRS1 response, ResultBase result)
        {
            foreach (var error in response.Errors)
            {
                switch (error.Code)
                {
                    case "ERFRU001":
                    case "ERFRU002":
                    case "ERFRU004":
                    case "ERFRU005":
                    case "ERFRU006":
                    case "ERFRU007":
                    case "ERFRU008":
                    case "ERFRU009":
                    case "ERFRU010":
                    case "ERFRU011":
                    case "ERFRU014":
                        goto case "InvalidInputData";
                    case "ERFRU003":
                        goto case "FareIdNoLongerValid";
                    case "ERGEN005":
                        result.ErrorMessages.Add("Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERMAI001":
                        result.ErrorMessages.Add("Mystifly is under maintenance!");
                        goto case "TechnicalError";

                    case "InvalidInputData":
                        result.Errors.Add(FlightError.InvalidInputData);
                        break;
                    case "FareIdNoLongerValid":
                        result.Errors.Add(FlightError.FareIdNoLongerValid);
                        break;
                    case "TechnicalError":
                        result.Errors.Add(FlightError.TechnicalError);
                        break;
                }
            }
        }
    }
}
