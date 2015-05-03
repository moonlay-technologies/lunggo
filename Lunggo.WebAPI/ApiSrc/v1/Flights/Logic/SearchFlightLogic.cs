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
                    FlightList = null
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
                request.AdultCount + request.ChildCount <= 9 &&
                request.InfantCount <= request.AdultCount &&
                (
                    (request.TripType == TripType.OneWay && request.TripInfos.Count == 1) ||
                    (request.TripType == TripType.Return && request.TripInfos.Count == 2) ||
                    (request.TripType == TripType.OpenJaw && request.TripInfos.Count > 1) ||
                    (request.TripType == TripType.Circle && request.TripInfos.Count > 2)
                ) &&
                request.TripInfos.TrueForAll(data => data.DepartureDate >= DateTime.Now);
        }

        private static FlightSearchApiResponse AssembleApiResponse(SearchFlightOutput searchServiceResponse, FlightSearchApiRequest request)
        {
            var apiResponse = new FlightSearchApiResponse
            {
                OriginalRequest = request,
            };
            if (searchServiceResponse.Itineraries == null)
            {
                apiResponse.FlightList = null;
                apiResponse.TotalFlightCount = 0;
            }
            else
            {
                apiResponse.SearchId = searchServiceResponse.SearchId;
                apiResponse.FlightList = MapItineraries(searchServiceResponse.Itineraries);
                apiResponse.TotalFlightCount = searchServiceResponse.Itineraries.Count;
            }
            return apiResponse;
        }

        private static List<FlightItineraryApi> MapItineraries(IEnumerable<FlightItineraryFare> itineraries)
        {
            var list = itineraries.Select(itin => new FlightItineraryApi
            {
                AdultCount = itin.AdultCount, 
                ChildCount = itin.ChildCount, 
                InfantCount = itin.InfantCount, 
                AdultTotalFare = itin.AdultTotalFare,
                ChildTotalFare = itin.ChildTotalFare, 
                InfantTotalFare = itin.InfantTotalFare, 
                Supplier = itin.Supplier, 
                RequireBirthDate = itin.RequireBirthDate,
                RequirePassport = itin.RequirePassport,
                RequireSameCheckIn = itin.RequireSameCheckIn, 
                CanHold = itin.CanHold,
                TripType = itin.TripType, 
                TotalFare = itin.TotalFare,
                PscFare = itin.PscFare, 
                FlightTrips = MapTrips(itin.FlightTrips), 
            }).ToList();
            for (var i = 0; i < list.Count; i++)
            {
                list[i].SequenceNo = i;
            }
            var orderedList = list.OrderBy(itin => itin.SequenceNo);
            return orderedList.ToList();
        }

        private static List<FlightTripApi> MapTrips(IEnumerable<FlightTripFare> trips)
        {
            var dict = DictionaryService.GetInstance();
            return trips.Select(trip => new FlightTripApi
            {
                FlightSegments = MapSegments(trip.FlightSegments), 
                OriginAirport = trip.OriginAirport,
                OriginCity = dict.GetAirportCity(trip.OriginAirport),
                OriginAirportName = dict.GetAirportName(trip.OriginAirport),
                DestinationAirport = trip.DestinationAirport,
                DestinationCity = dict.GetAirportCity(trip.DestinationAirport),
                DestinationAirportName = dict.GetAirportName(trip.DestinationAirport),
                DepartureDate = trip.DepartureDate, 
                TotalDuration = CalculateTotalDuration(trip),
                Airlines = GetAirlineList(trip),
                TotalTransit = CalculateTotalTransit(trip),
                Transits = MapTransitDetails(trip)
            }).ToList();
        }

        private static List<FlightSegmentApi> MapSegments(IEnumerable<FlightSegmentFare> segments)
        {
            var dict = DictionaryService.GetInstance();
            return segments.Select(segment => new FlightSegmentApi
            {
                DepartureAirport = segment.DepartureAirport,
                DepartureCity = dict.GetAirportCity(segment.DepartureAirport),
                DepartureAirportName = dict.GetAirportName(segment.DepartureAirport),
                DepartureTime = segment.DepartureTime,
                ArrivalAirport = segment.ArrivalAirport,
                ArrivalCity = dict.GetAirportCity(segment.ArrivalAirport),
                ArrivalAirportName = dict.GetAirportName(segment.ArrivalAirport),
                ArrivalTime = segment.ArrivalTime,
                Duration = segment.Duration,
                AirlineCode = segment.AirlineCode,
                FlightNumber = segment.FlightNumber,
                OperatingAirlineCode = segment.OperatingAirlineCode,
                AircraftCode = segment.AircraftCode,
                CabinClass = segment.CabinClass,
                StopQuantity = segment.StopQuantity,
                FlightStops = segment.FlightStops,
                Rbd = segment.Rbd,
                RemainingSeats = segment.RemainingSeats
            }).ToList();
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

        private static List<Airline> GetAirlineList(FlightTripFare trip)
        {
            var dict = DictionaryService.GetInstance();
            var segments = trip.FlightSegments;
            var airlineCodes = segments.Select(segment => segment.AirlineCode);
            var airlines = airlineCodes.Distinct().Select(code => new Airline
            {
                Code = code,
                Name = dict.GetAirlineName(code)
            });
            return airlines.ToList();
        }

        private static List<Transit> MapTransitDetails(FlightTripFare trip)
        {
            var segments = trip.FlightSegments;
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

        private static int CalculateTotalTransit(FlightTripFare trip)
        {
            var segments = trip.FlightSegments;
            var transit = segments.Count() - 1;
            var stop = segments.Sum(segment => segment.StopQuantity);
            return transit + stop;
        }

        private static SearchFlightInput PreprocessServiceRequest(FlightSearchApiRequest request)
        {
            var searchConditions = new SearchFlightConditions
            {
                AdultCount = request.AdultCount,
                ChildCount = request.ChildCount,
                InfantCount = request.InfantCount,
                CabinClass = request.CabinClass,
                TripInfos = request.TripInfos
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