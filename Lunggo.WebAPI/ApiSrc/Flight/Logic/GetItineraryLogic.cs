using System;
using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Lunggo.WebAPI.ApiSrc.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.Flight.Logic
{
    public static partial class FlightLogic
    {
        public static ApiResponseBase GetItinerary(string token)
        {
            try
            {
                var service = FlightService.GetInstance();
                var itinerary = service.GetItineraryForDisplay(token);
                var expiryTime = service.GetItineraryExpiry(token);
                var apiResponse = AssembleApiResponse(itinerary, expiryTime);
                return apiResponse;
            }
            catch
            {
                return ApiResponseBase.Error500();
            }
        }

        private static ApiResponseBase AssembleApiResponse(FlightItineraryForDisplay itinerary, DateTime? expiryTime)
        {
            if (itinerary == null)
                return new FlightItineraryApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERFITI01"
                };

            return new FlightItineraryApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Itinerary = itinerary,
                ExpiryTime = expiryTime
            };
        }
    }
}