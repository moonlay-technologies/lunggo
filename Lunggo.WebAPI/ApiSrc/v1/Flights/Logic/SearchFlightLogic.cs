using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Framework.Redis;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using StackExchange.Redis;
using CabinClass = Lunggo.ApCommon.Flight.Constant.CabinClass;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightSearchApiResponse SearchFlights(FlightSearchApiRequest request)
        {
            if (IsValid(request))
            {
                var searchId = FlightSearchIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
                var searchServiceRequest = PreprocessServiceRequest(request);
                var searchServiceResponse = FlightService.GetInstance().SearchFlight(searchServiceRequest);
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
                apiResponse.FlightList = MapItineraries(searchServiceResponse.Itineraries);
                apiResponse.TotalFlightCount = searchServiceResponse.Itineraries.Count;
            }
            return apiResponse;
        }

        private static List<FlightItineraryApi> MapItineraries(IEnumerable<FlightItineraryFare> itineraries)
        {
            return itineraries.Select(itin => new FlightItineraryApi
            {
                AdultCount = itin.AdultCount, 
                ChildCount = itin.ChildCount, 
                InfantCount = itin.InfantCount, 
                AdultTotalFare = itin.AdultTotalFare,
                ChildTotalFare = itin.ChildTotalFare, 
                InfantTotalFare = itin.InfantTotalFare, 
                FareId = itin.FareId,
                Supplier = itin.Supplier, 
                RequireBirthDate = itin.RequireBirthDate,
                RequirePassport = itin.RequirePassport,
                RequireSameCheckIn = itin.RequireSameCheckIn, 
                CanHold = itin.CanHold,
                TripType = itin.TripType, 
                TotalFare = itin.TotalFare,
                PscFare = itin.PscFare, 
                FlightTrips = MapTrips(itin.FlightTrips), 
                Airlines = GetAirlineList(itin),
                TotalTransit = CalculateTotalTransit(itin),
                Transits = MapTransitDetails(itin)
            }).ToList();
        }

        private static List<FlightTripFare> MapTrips(IEnumerable<FlightTripFare> trips)
        {
            return trips.Select(trip => new FlightTripApi
            {
                FlightSegments = trip.FlightSegments, 
                OriginAirport = trip.OriginAirport, 
                DestinationAirport = trip.DestinationAirport, 
                DepartureDate = trip.DepartureDate, 
                TotalDuration = CalculateTotalDuration(trip)
            }).Cast<FlightTripFare>().ToList();
        }

        private static TimeSpan CalculateTotalDuration(FlightTripFare trip)
        {
            var segments = trip.FlightSegments;
            var totalFlightDuration = new TimeSpan();
            var totalTransitDuration = new TimeSpan();
            for (var i = 0; i < segments.Count; i++)
            {
                totalFlightDuration += segments[i].Duration;
                if (i != 0)
                    totalTransitDuration += segments[i].DepartureTime - segments[i - 1].ArrivalTime;
            }
            return totalFlightDuration + totalTransitDuration;
        }

        private static List<Airline> GetAirlineList(FlightItineraryFare itin)
        {
            var dict = DictionaryService.GetInstance();
            var segments = itin.FlightTrips.SelectMany(trip => trip.FlightSegments);
            var airlineCodes = segments.Select(segment => segment.AirlineCode);
            var airlines = airlineCodes.Distinct().Select(code => new Airline
            {
                Code = code,
                Name = dict.GetAirlineName(code)
            });
            return airlines.ToList();
        }

        private static List<Transit> MapTransitDetails(FlightItineraryFare itin)
        {
            var segments = itin.FlightTrips.SelectMany(trip => trip.FlightSegments).ToList();
            var result = new List<Transit>();
            for (var i = 0; i < segments.Count; i++)
            {
                if (segments[i].FlightStops != null)
                {
                    result.AddRange(segments[i].FlightStops.Select(stop => new Transit
                    {
                        IsStop = true,
                        Airport = stop.Airport,
                        ArrivalTime = stop.ArrivalTime,
                        DepartureTime = stop.DepartureTime,
                    }));
                }
                if (i != 0)
                {
                    result.Add(new Transit
                    {
                        IsStop = false,
                        Airport = segments[i].DepartureAirport,
                        ArrivalTime = segments[i - 1].ArrivalTime,
                        DepartureTime = segments[i].DepartureTime
                    });
                }
            }
            return result;
        }

        private static int CalculateTotalTransit(FlightItineraryFare itin)
        {
            var segments = itin.FlightTrips.SelectMany(trip => trip.FlightSegments).ToList();
            var transit = segments.Count() - 1;
            var stop = segments.Sum(segment => segment.StopQuantity);
            return transit + stop;
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