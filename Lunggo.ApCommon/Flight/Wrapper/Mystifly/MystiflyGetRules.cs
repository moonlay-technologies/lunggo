using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Flight.Wrapper.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal GetRulesResult GetRules(string fareId)
        {
            var request = new AirRulesRQ1
            {
                FareSourceCode = FlightService.IdUtil.GetCoreId(fareId),
                SessionId = Client.SessionId,
                Target = Client.Target,
                ExtensionData = null
            };
            var result = new GetRulesResult();
            var retry = 0;
            var done = false;
            while (!done)
            {
                var response = Client.FareRules1_1(request);
                done = true;
                if (response.Success && !response.Errors.Any() ||
                    response.Errors.Count() == 1 && response.Errors.Single().Code == "ERFRU012")
                {
                    result = MapResult(response);
                    result.IsSuccess = true;
                    result.Errors = null;
                    result.ErrorMessages = null;
                }
                else
                {
                    if (response.Errors.Any())
                    {
                        result.Errors = new List<FlightError>();
                        foreach (var error in response.Errors)
                        {
                            if (error.Code == "ERFRU001" || error.Code == "ERFRU013")
                            {
                                Client.CreateSession();
                                request.SessionId = Client.SessionId;
                                retry++;
                                if (retry <= 3)
                                {
                                    done = false;
                                    break;
                                }
                            }
                        }
                        if (done)
                            MapError(response, result);
                    }
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        private static GetRulesResult MapResult(AirRulesRS1 response)
        {
            var dict = DictionaryService.GetInstance();
            var result = new GetRulesResult();
            if (response.FareRules != null)
            {
                result.AirlineRules = response.FareRules.Select(fareRule => new AirlineRules
                {
                    AirlineCode = fareRule.Airline,
                    AirlineName = dict.GetAirlineName(fareRule.Airline),
                    DepartureAirport = fareRule.CityPair.Substring(0, 3),
                    DepartureAirportName = dict.GetAirportName(fareRule.CityPair.Substring(0, 3)),
                    DepartureCity = dict.GetAirportCity(fareRule.CityPair.Substring(0, 3)),
                    DepartureCountry = dict.GetAirportCountry(fareRule.CityPair.Substring(0, 3)),
                    ArrivalAirport = fareRule.CityPair.Substring(3, 3),
                    ArrivalAirportName = dict.GetAirportName(fareRule.CityPair.Substring(3, 3)),
                    ArrivalCity = dict.GetAirportCity(fareRule.CityPair.Substring(3, 3)),
                    ArrivalCountry = dict.GetAirportCountry(fareRule.CityPair.Substring(3, 3)),
                    Rules = fareRule.RuleDetails.Select(rule => rule.Rules).ToList()
                }).ToList();
            }
            else
            {
                result.AirlineRules = new List<AirlineRules>();
            }
            if (response.BaggageInfos != null)
            {
                result.BaggageRules = response.BaggageInfos.Select(baggageRule => new BaggageRules
                {
                    AirlineCode = baggageRule.FlightNo.Substring(0, 2),
                    AirlineName = dict.GetAirlineName(baggageRule.FlightNo.Substring(0, 2)),
                    FlightNumber = baggageRule.FlightNo.Substring(2),
                    DepartureAirport = baggageRule.Departure,
                    DepartureAirportName = dict.GetAirportName(baggageRule.Departure),
                    DepartureCity = dict.GetAirportCity(baggageRule.Departure),
                    DepartureCountry = dict.GetAirportCountry(baggageRule.Departure),
                    ArrivalAirport = baggageRule.Arrival,
                    ArrivalAirportName = dict.GetAirportName(baggageRule.Arrival),
                    ArrivalCity = dict.GetAirportCity(baggageRule.Arrival),
                    ArrivalCountry = dict.GetAirportCountry(baggageRule.Arrival),
                    Baggage = baggageRule.Baggage
                }).ToList();
            }
            else
            {
                result.BaggageRules = new List<BaggageRules>();
            }
            return result;
        }

        private static void MapError(AirRulesRS1 response, ResultBase result)
        {
            foreach (var error in response.Errors)
            {
                switch (error.Code)
                {
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
                    case "ERFRU001":
                    case "ERFRU013":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("Invalid account information!"))
                            result.ErrorMessages.Add("Invalid account information!");
                        goto case "TechnicalError";
                    case "ERGEN005":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("Unexpected error on the other end!"))
                            result.ErrorMessages.Add("Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERMAI001":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("Mystifly is under maintenance!"))
                            result.ErrorMessages.Add("Mystifly is under maintenance!");
                        goto case "TechnicalError";

                    case "InvalidInputData":
                        if (!result.Errors.Contains(FlightError.InvalidInputData))
                            result.Errors.Add(FlightError.InvalidInputData);
                        break;
                    case "FareIdNoLongerValid":
                        if (!result.Errors.Contains(FlightError.FareIdNoLongerValid))
                            result.Errors.Add(FlightError.FareIdNoLongerValid);
                        break;
                    case "TechnicalError":
                        if (!result.Errors.Contains(FlightError.TechnicalError))
                            result.Errors.Add(FlightError.TechnicalError);
                        break;
                }
            }
        }
    }
}
