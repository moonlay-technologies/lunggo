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
                    SearchId = null,
                    Flights = new List<Flight>(),
                    GrantedRequests = new List<int>(),
                    ExpiryTime = null,
                    MaxRequest = 0,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERFSEA01"
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
                SearchId = searchServiceResponse.SearchId,
                Flights = searchServiceResponse.ItineraryLists.Select(itinList => new Flight
                {
                    Count = itinList.Count,
                    Itineraries = itinList
                }).ToList(),
                ExpiryTime = searchServiceResponse.ExpiryTime,
                GrantedRequests = searchServiceResponse.SearchedSuppliers,
                MaxRequest = searchServiceResponse.TotalSupplier,
                StatusCode = HttpStatusCode.OK
            };
            return apiResponse;
        }

        private static SearchFlightInput PreprocessServiceRequest(FlightSearchApiRequest request)
        {
            var supplierIds = request.Requests;
            var searchServiceRequest = new SearchFlightInput
            {
                SearchId = request.SearchId,
                RequestedSupplierIds = supplierIds
            };
            return searchServiceRequest;
        }
    }
}