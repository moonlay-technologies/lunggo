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
    public class GetFlightLogic
    {
        public static FlightSearchApiResponse GetFlights(FlightSearchApiRequest request)
        {
            var searchServiceRequest = PreprocessServiceRequest(request);
            var searchServiceResponse = FlightService.GetInstance().SearchFlight(searchServiceRequest);
            var apiResponse = AssembleApiResponse(searchServiceResponse, request);
            return apiResponse;
        }

        private static FlightSearchApiResponse AssembleApiResponse(SearchFlightOutput searchServiceResponse, FlightSearchApiRequest request)
        {
            var apiResponse = new FlightSearchApiResponse
            {
                InitialRequest = request,
                SearchId = null,
                TotalFlightCount = searchServiceResponse.Itineraries.Count,
                FlightList = searchServiceResponse.Itineraries
            };
            return apiResponse;
        }

        private static SearchFlightInput PreprocessServiceRequest(FlightSearchApiRequest request)
        {
            var originDestinationInformation = new List<OriginDestinationInfo>
            {
                new OriginDestinationInfo
                {
                    OriginAirport = AssignOriginAirport(request.Origin),
                    DestinationAirport = AssignDestinationAirport(request.Destination),
                    DepartureDate = AssignDepartureDate(request.Date)
                }
            };
            var searchConditions = new SearchFlightConditions
            {
                AdultCount = AssignAdultPassenger(request.Adult),
                ChildCount = AssignChildPassenger(request.Child),
                InfantCount = AssignInfantPassenger(request.Infant),
                CabinClass = AssignCabinClass(request.Cabin),
                OriDestInfos = originDestinationInformation
            };
            var searchServiceRequest = new SearchFlightInput
            {
                Conditions = searchConditions,
                IsReturnSeparated = false,
                IsDateFlexible = false,
                TripType = TripType.OneWay
            };
            return searchServiceRequest;
        }

        private static string AssignOriginAirport(string origin)
        {
            if (origin == null)
                return "CGK";
            else
                return origin;
        }

        private static string AssignDestinationAirport(string destination)
        {
            if (destination == null)
                return "CGK";
            else
                return destination;
        }

        private static DateTime AssignDepartureDate(DateTime? date)
        {
            if (date == null || (DateTime)date < DateTime.Now)
                return DateTime.Now;
            else
                return (DateTime)date;
        }

        private static int AssignAdultPassenger(int? adult)
        {
            if (adult == null || (int)adult < 1)
                return 0;
            else
                return (int)adult;
        }

        private static int AssignChildPassenger(int? child)
        {
            if (child == null || (int)child < 0)
                return 0;
            else
                return (int)child;
        }

        private static int AssignInfantPassenger(int? infant)
        {
            if (infant == null || (int)infant < 0)
                return 0;
            else
                return (int)infant;
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