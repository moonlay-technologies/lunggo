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
            if (IsValid(request))
            {
                var revalidateServiceRequest = PreprocessServiceRequest(request);
                var revalidateServiceResponse = FlightService.GetInstance().RevalidateFlight(revalidateServiceRequest);
                var apiResponse = AssembleApiResponse(revalidateServiceResponse, request);
                return apiResponse;
            }
            else
            {
                return new FlightRevalidateApiResponse
                {
                    IsValid = false,
                    Itinerary = null,
                    OriginalRequest = request
                };
            }
        }

        private static RevalidateFlightInput PreprocessServiceRequest(FlightRevalidateApiRequest request)
        {
            var service = FlightService.GetInstance();
            var itin = service.GetItineraryFromCache(request.Hash);
            return new RevalidateFlightInput { FareId = itin.FareId };
        }

        private static bool IsValid(FlightRevalidateApiRequest request)
        {
            return
                request.Hash != null;
        }

        private static FlightRevalidateApiResponse AssembleApiResponse(RevalidateFlightOutput revalidateServiceResponse, FlightRevalidateApiRequest request)
        {
            if (revalidateServiceResponse.IsSuccess)
            {
                if (revalidateServiceResponse.IsValid)
                {
                    return new FlightRevalidateApiResponse
                    {
                        IsValid = true,
                        IsOtherFareAvailable = false,
                        Itinerary = revalidateServiceResponse.Itinerary,
                        OriginalRequest = request
                    };
                }
                else
                {
                    if (revalidateServiceResponse.Itinerary == null)
                    {
                        return new FlightRevalidateApiResponse
                        {
                            IsValid = false,
                            IsOtherFareAvailable = false,
                            Itinerary = null,
                            OriginalRequest = request
                        };
                    }
                    else
                    {
                        return new FlightRevalidateApiResponse
                        {
                            IsValid = false,
                            IsOtherFareAvailable = true,
                            Itinerary = revalidateServiceResponse.Itinerary,
                            OriginalRequest = request
                        };
                    }
                }
            }
            else
            {
                return new FlightRevalidateApiResponse
                {
                    IsValid = false,
                    IsOtherFareAvailable = false,
                    Itinerary = null,
                    OriginalRequest = request
                };
            }
        }
    }
}