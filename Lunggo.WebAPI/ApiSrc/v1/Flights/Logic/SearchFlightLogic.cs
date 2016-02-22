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
                    FlightList = new List<FlightItineraryForDisplay>(),
                    GrantedRequests = new List<int>(),
                    ExpiryTime = null,
                    MaxRequest = 0,
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Invalid search_id or requests format."
                };
            }
        }

        private static bool IsValid(FlightSearchApiRequest request)
        {
            return FlightService.GetInstance().IsSearchIdValid(request.SearchId);
        }

        private static FlightSearchApiResponse AssembleApiResponse(SearchFlightOutput searchServiceResponse, FlightSearchApiRequest request)
        {
            var apiResponse = new FlightSearchApiResponse
            {
                OriginalRequest = request,
                SearchId = searchServiceResponse.SearchId,
                FlightList = searchServiceResponse.Itineraries,
                TotalFlightCount = searchServiceResponse.Itineraries.Count,
                ReturnFlightList = searchServiceResponse.ReturnItineraries,
                TotalReturnFlightCount = searchServiceResponse.ReturnItineraries == null ? null : (int?) searchServiceResponse.ReturnItineraries.Count,
                ExpiryTime = searchServiceResponse.ExpiryTime,
                GrantedRequests = searchServiceResponse.SearchedSuppliers,
                MaxRequest = searchServiceResponse.TotalSupplier,
                StatusCode = HttpStatusCode.OK,
                StatusMessage = "Success."
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