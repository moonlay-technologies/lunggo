using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using FlightService = Lunggo.ApCommon.Flight.Service.FlightService;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightRevalidateApiResponse RevalidateFlight(FlightRevalidateApiRequest request)
        {
            var service = FlightService.GetInstance();
            var revalidateServiceRequest = PreprocessServiceRequest(request);
            var revalidateServiceResponse = service.RevalidateFlight(revalidateServiceRequest);
            var apiResponse = AssembleApiResponse(revalidateServiceResponse, request);
            return apiResponse;
        }

        private static RevalidateFlightInput PreprocessServiceRequest(FlightRevalidateApiRequest request)
        {
            return new RevalidateFlightInput
            {
                SearchId = request.SearchId,
                ItinIndex = request.ItinIndex,
                Token = request.Token
            };
        }

        private static FlightRevalidateApiResponse AssembleApiResponse(RevalidateFlightOutput revalidateServiceResponse, FlightRevalidateApiRequest request)
        {
            if (revalidateServiceResponse.IsSuccess)
            {
                if (revalidateServiceResponse.IsValid)
                {
                    return new FlightRevalidateApiResponse
                    {
                        Token = revalidateServiceResponse.Token,
                        IsValid = true,
                        IsOtherFareAvailable = null,
                        NewFare = null,
                        OriginalRequest = request
                    };
                }
                else
                {
                    if (revalidateServiceResponse.NewFare != null)
                    {
                        return new FlightRevalidateApiResponse
                        {
                            Token = revalidateServiceResponse.Token,
                            IsValid = false,
                            IsOtherFareAvailable = true,
                            NewFare = revalidateServiceResponse.NewFare,
                            OriginalRequest = request
                        };
                    }
                    else
                    {
                        return new FlightRevalidateApiResponse
                        {
                            Token = null,
                            IsValid = false,
                            IsOtherFareAvailable = false,
                            NewFare = null,
                            OriginalRequest = request
                        };
                    }
                }
            }
            else
            {
                return new FlightRevalidateApiResponse
                {
                    Token = null,
                    IsValid = false,
                    IsOtherFareAvailable = false,
                    NewFare = 0,
                    OriginalRequest = request
                };
            }
        }

        private static bool IsValid(FlightRevalidateApiRequest request)
        {
            return
                request != null &&
                (request.SearchId != null || request.Token != null);
        }
    }
}