using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Redis;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using StackExchange.Redis;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightSearchApiResponse SearchFlights(FlightSearchApiRequest request)
        {
            if (IsValid(request))
            {
                var searchServiceRequest = PreprocessServiceRequest(request);
                var searchServiceResponse = FlightService.GetInstance().SearchFlight(searchServiceRequest);
                var apiResponse = AssembleApiResponse(searchServiceResponse, request);
                return apiResponse;
            }
            else
            {
                return new FlightSearchApiResponse
                {
                    SearchId = null,
                    OriginalRequest = request,
                    TotalFlightCount = 0,
                    FlightList = new List<FlightItineraryApi>()
                };
            }
        }

        private static bool IsValid(FlightSearchApiRequest request)
        {
            return
                request != null &&
                request.AdultCount >= 1 &&
                request.ChildCount >= 0 &&
                request.InfantCount >= 0 &&
                request.AdultCount + request.ChildCount + request.InfantCount <= 9 &&
                request.InfantCount <= request.AdultCount &&
                (
                    (request.TripType == TripType.OneWay && request.Trips.Count == 1) ||
                    (request.TripType == TripType.Return && request.Trips.Count == 2) ||
                    (request.TripType == TripType.OpenJaw && request.Trips.Count > 1) ||
                    (request.TripType == TripType.Circle && request.Trips.Count > 2)
                ) &&
                request.Trips.TrueForAll(data => data.DepartureDate >= DateTime.UtcNow);
        }

        private static FlightSearchApiResponse AssembleApiResponse(SearchFlightOutput searchServiceResponse, FlightSearchApiRequest request)
        {
            var apiResponse = new FlightSearchApiResponse
            {
                OriginalRequest = request,
                SearchId = searchServiceResponse.SearchId,
                FlightList = searchServiceResponse.Itineraries,
                TotalFlightCount = searchServiceResponse.Itineraries.Count,
                ExpiryTime = searchServiceResponse.ExpiryTime
            };
            return apiResponse;
        }

        private static SearchFlightInput PreprocessServiceRequest(FlightSearchApiRequest request)
        {
            var searchConditions = new SearchFlightConditions
            {
                AdultCount = request.AdultCount,
                ChildCount = request.ChildCount,
                InfantCount = request.InfantCount,
                CabinClass = request.CabinClass,
                Trips = request.Trips
            };
            var searchServiceRequest = new SearchFlightInput
            {
                Conditions = searchConditions,
                IsDateFlexible = false,
            };
            return searchServiceRequest;
        }
    }
}