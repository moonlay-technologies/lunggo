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
                var service = FlightService.GetInstance();
                if (request.HashKey == null)
                    request.HashKey = service.SaveItineraryToCache(request.SearchId, request.ItinIndex);
                var revalidateServiceRequest = PreprocessServiceRequest(request);
                var revalidateServiceResponse = service.RevalidateFlight(revalidateServiceRequest);
                if (!revalidateServiceResponse.IsValid && revalidateServiceResponse.Itinerary != null)
                    UpdateItinerary(revalidateServiceResponse.Itinerary, request.HashKey);
                var apiResponse = AssembleApiResponse(revalidateServiceResponse, request);
                return apiResponse;
            }
            else
            {
                return new FlightRevalidateApiResponse
                {
                    HashKey = null,
                    IsValid = false,
                    IsOtherFareAvailable = false,
                    NewFare = 0,
                    OriginalRequest = request
                };
            }
        }

        private static RevalidateFlightInput PreprocessServiceRequest(FlightRevalidateApiRequest request)
        {
            var service = FlightService.GetInstance();
            var itin = service.GetItineraryFromCache(request.HashKey);
            return new RevalidateFlightInput
            {
                FareId = itin.FareId,
                TripInfos = itin.FlightTrips.Select(trip => new FlightTripInfo
                {
                    OriginAirport = trip.OriginAirport,
                    DestinationAirport = trip.DestinationAirport,
                    DepartureDate = trip.DepartureDate,
                }).ToList()
            };
        }

        private static void UpdateItinerary(FlightItineraryFare itin, string hash)
        {
            var service = FlightService.GetInstance();
            service.SaveItineraryToCache(itin, hash);
        }

        private static FlightRevalidateApiResponse AssembleApiResponse(RevalidateFlightOutput revalidateServiceResponse, FlightRevalidateApiRequest request)
        {
            if (revalidateServiceResponse.IsSuccess)
            {
                if (revalidateServiceResponse.IsValid)
                {
                    return new FlightRevalidateApiResponse
                    {
                        HashKey = request.HashKey,
                        IsValid = true,
                        IsOtherFareAvailable = null,
                        NewFare = null,
                        OriginalRequest = request
                    };
                }
                else
                {
                    if (revalidateServiceResponse.Itinerary == null)
                    {
                        return new FlightRevalidateApiResponse
                        {
                            HashKey = null,
                            IsValid = false,
                            IsOtherFareAvailable = false,
                            NewFare = null,
                            OriginalRequest = request
                        };
                    }
                    else
                    {
                        return new FlightRevalidateApiResponse
                        {
                            HashKey = request.HashKey,
                            IsValid = false,
                            IsOtherFareAvailable = true,
                            NewFare = revalidateServiceResponse.Itinerary.LocalPrice,
                            OriginalRequest = request
                        };
                    }
                }
            }
            else
            {
                return new FlightRevalidateApiResponse
                {
                    HashKey = null,
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
                (request.SearchId != null || request.HashKey != null);
        }
    }
}