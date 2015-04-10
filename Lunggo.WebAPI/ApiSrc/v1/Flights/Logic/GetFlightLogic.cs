using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightSearchApiResponse GetFlights(FlightSearchApiRequest request)
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
                    FlightList = null
                };
            }
            
        }

        private static bool IsValid(FlightSearchApiRequest request)
        {
            return
                request != null &&
                request.Adult >= 1 &&
                request.Child >= 0 &&
                request.Infant >= 0 &&
                request.Adult + request.Child <= 9 &&
                request.Cabin != null;
            /*
                (
                    (request.Type == "One" && request.OriDestDate.Count == 1) ||
                    (request.Type == "Ret" && request.OriDestDate.Count == 2) ||
                    (request.Type == "Mul" && request.OriDestDate.Count >= 1)
                ) &&
                request.OriDestDate.TrueForAll(data => data.Date >= DateTime.Now);
                */
        }

        private static FlightSearchApiResponse AssembleApiResponse(SearchFlightOutput searchServiceResponse, FlightSearchApiRequest request)
        {
            var apiResponse = new FlightSearchApiResponse
            {
                OriginalRequest = request,
                SearchId = null
            };
            if (searchServiceResponse.Itineraries == null)
            {
                apiResponse.FlightList = null;
                apiResponse.TotalFlightCount = 0;
            }
            else
            {
                apiResponse.FlightList = searchServiceResponse.Itineraries;
                apiResponse.TotalFlightCount = searchServiceResponse.Itineraries.Count;
            }
            return apiResponse;
        }

        private static SearchFlightInput PreprocessServiceRequest(FlightSearchApiRequest request)
        {
            var searchConditions = new SearchFlightConditions
            {
                AdultCount = request.Adult,
                ChildCount = request.Child,
                InfantCount = request.Infant,
                CabinClass = AssignCabinClass(request.Cabin),
                OriDestInfos = new List<OriginDestinationInfo>
                {
                    new OriginDestinationInfo
                    {
                        OriginAirport = request.Ori,
                        DestinationAirport = request.Dest,
                        DepartureDate = request.Date
                    }
                }
                /*
                OriDestInfos = request.OriDestDate.Select(data => new OriginDestinationInfo
                {
                    OriginAirport = data.Ori,
                    DestinationAirport = data.Dest,
                    DepartureDate = data.Date
                }).ToList(),
                 */
            };
            var searchServiceRequest = new SearchFlightInput
            {
                Conditions = searchConditions,
                IsDateFlexible = false,
            };
            return searchServiceRequest;
        }

        private static CabinClass AssignCabinClass(string cabin)
        {
            switch (cabin)
            {
                case "E":
                    return CabinClass.Economy;
                case "B":
                    return CabinClass.Business;
                case "F":
                    return CabinClass.First;
                default:
                    return CabinClass.Economy;
            }
        }
    }
}