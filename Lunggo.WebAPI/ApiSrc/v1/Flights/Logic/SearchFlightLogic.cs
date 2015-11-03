using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

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
                    FlightList = new List<FlightItineraryForDisplay>()
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
                request.Trips.TrueForAll(data => data.DepartureDate >= DateTime.UtcNow.Date);
        }

        private static FlightSearchApiResponse AssembleApiResponse(SearchFlightOutput searchServiceResponse, FlightSearchApiRequest request)
        {
            var apiResponse = new FlightSearchApiResponse
            {
                OriginalRequest = request,
                SearchId = searchServiceResponse.SearchId,
                FlightList = searchServiceResponse.Itineraries,
                TotalFlightCount = searchServiceResponse.Itineraries.Count,
                ExpiryTime = searchServiceResponse.ExpiryTime,
                GrantedRequests = searchServiceResponse.SearchedSuppliers,
                MaxRequest = searchServiceResponse.TotalSupplier
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
                RequestedSupplierIds = request.Requests,
                IsDateFlexible = false
            };
            return searchServiceRequest;
        }
    }
}