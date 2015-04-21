﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override RevalidateFareResult RevalidateFare(RevalidateConditions conditions)
        {
            using (var client = new MystiflyClientHandler())
            {
                var request = new AirRevalidateRQ
                {
                    FareSourceCode = conditions.FareId,
                    SessionId = client.SessionId,
                    Target = MystiflyClientHandler.Target,
                    ExtensionData = null
                };
                
                var result = new RevalidateFareResult();
                var retry = 0;
                var done = false;
                while (!done)
                {                                                            
                    done = true;
                    var response = client.AirRevalidate(request);
                    if (!response.Errors.Any() && response.Success)
                    {
                        result = MapResult(response, conditions);
                        result.IsSuccess = true;
                    }
                    else
                    {
                        if (response.Errors.Any())
                        {
                            result.Errors = new List<FlightError>();
                            result.ErrorMessages = new List<string>();
                            foreach (var error in response.Errors)
                            {
                                if (error.Code == "ERREV002")
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
                        result.IsSuccess = false;
                    }
                }
                return result;
            }
        }

        private static RevalidateFareResult MapResult(AirRevalidateRS response, RevalidateConditions conditions)
        {
            var result = new RevalidateFareResult();
            CheckFareValidity(response, result);
            if (response.PricedItineraries.Any())
                result.Itinerary = MapFlightFareItinerary(response.PricedItineraries[0], conditions);
            return result;
        }

        private static void CheckFareValidity(AirRevalidateRS response, RevalidateFareResult result)
        {
            result.IsValid = response.IsValid;
        }

        private static void MapError(AirRevalidateRS response, ResultBase result)
        {
            foreach (var error in response.Errors)
            {
                switch (error.Code)
                {
                    case "ERREV001" :
                    case "ERREV003" :
                        goto case "InvalidInputData";
                    case "ERREV004" :
                    case "ERREV005" :
                        goto case "FareIdNoLongerValid";
                    case "ERGEN004":
                        result.ErrorMessages.Add("Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERMAI001":
                        result.ErrorMessages.Add("Mystifly is under maintenance!");
                        goto case "TechnicalError";

                    case "InvalidInputData" :
                        result.Errors.Add(FlightError.InvalidInputData);
                        break;
                    case "FareIdNoLongerValid" :
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