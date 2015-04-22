using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Sequence;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightSearchApiResponse GetFlights(FlightSearchApiRequest request)
        {
            if (IsValid(request))
            {
                var searchId = FlightSearchIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
                var searchServiceRequest = PreprocessServiceRequest(request);
                var searchServiceResponse = FlightService.GetInstance().SearchFlight(searchServiceRequest);
                for (var i = 0; i < searchServiceResponse.Itineraries.Count; i++)
                {
                    DictionaryService.GetInstance().ItineraryDict.Add(searchId + i, searchServiceResponse.Itineraries[i]);
                }
                var apiResponse = AssembleApiResponse(searchServiceResponse, request, searchId);
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

        private static FlightSearchApiResponse AssembleApiResponse(SearchFlightOutput searchServiceResponse, FlightSearchApiRequest request, string searchId)
        {
            var apiResponse = new FlightSearchApiResponse
            {
                OriginalRequest = request,
                SearchId = searchId
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
                TripInfos = new List<TripInfo>
                {
                    new TripInfo
                    {
                        OriginAirport = request.Ori,
                        DestinationAirport = request.Dest,
                        DepartureDate = request.Date
                    }
                }
                /*
                TripInfos = request.OriDestDate.Select(data => new TripInfo
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