using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public static partial class FlightLogic
    {
        public static FlightSearchApiResponse SearchFlights(FlightSearchApiRequest request)
        {
            try
            {
                if (IsValid(request))
                {
                    var searchServiceRequest = PreprocessServiceRequest(request);
                    var searchServiceResponse = FlightService.GetInstance().SearchFlight(searchServiceRequest);
                    var apiResponse = AssembleApiResponse(searchServiceResponse);
                    return apiResponse;
                }
                else
                {
                    return new FlightSearchApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorCode = "ERFSEA01"
                    };
                }
            }
            catch
            {
                return new FlightSearchApiResponse
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "ERFSEA99"
                };
            }
        }

        private static bool IsValid(FlightSearchApiRequest request)
        {
            return FlightService.GetInstance().IsSearchIdValid(request.SearchId);
        }

        private static FlightSearchApiResponse AssembleApiResponse(SearchFlightOutput searchServiceResponse)
        {
            var apiResponse = new FlightSearchApiResponse
            {
                Flights = searchServiceResponse.ItineraryLists.Select(itinList => new Flight
                {
                    Count = itinList.Count,
                    Itineraries = itinList
                }).ToList(),
                Combos = searchServiceResponse.Combos,
                ExpiryTime = searchServiceResponse.ExpiryTime,
                Progress = searchServiceResponse.Progress,
                StatusCode = HttpStatusCode.OK
            };
            return apiResponse;
        }

        private static SearchFlightInput PreprocessServiceRequest(FlightSearchApiRequest request)
        {
            var searchServiceRequest = new SearchFlightInput
            {
                SearchId = request.SearchId,
                Progress = request.Progress
            };
            return searchServiceRequest;
        }
    }
}