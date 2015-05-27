using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Redis;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Logic
{
    public partial class FlightLogic
    {
        public static FlightFilterApiResponse FilterFlights(FlightFilterApiRequest request)
        {
            var unfilteredItineraries = GetUnfilteredItineraries(request);
            var filteredItineraries = FilterItineraries(unfilteredItineraries, request);
            var apiResponse = AssembleApiResponse(filteredItineraries, request);
            return apiResponse;
        }

        private static List<FlightItineraryApi> FilterItineraries(IEnumerable<FlightItineraryApi> unfilteredItineraries, FlightFilterApiRequest request)
        {
            var filteredItins = FilterByPriceRange(unfilteredItineraries, request);
            filteredItins = FilterByAirline(filteredItins, request);
            filteredItins = FilterByDepartureTimeSpan(filteredItins, request);
            filteredItins = FilterByArrivalTimeSpan(filteredItins, request);
            filteredItins = FilterByTotalTransit(filteredItins, request);
            return filteredItins.ToList();
        }

        private static IEnumerable<FlightItineraryApi> FilterByPriceRange(IEnumerable<FlightItineraryApi> unfilteredItineraries, FlightFilterApiRequest request)
        {
            return unfilteredItineraries.Where(
                itin =>
                    itin.TotalFare >= request.PriceRange.Start && itin.TotalFare <= request.PriceRange.End);
        }

        private static IEnumerable<FlightItineraryApi> FilterByAirline(IEnumerable<FlightItineraryApi> filteredItins, FlightFilterApiRequest request)
        {
            return filteredItins.Where(
                itin =>
                    itin.FlightTrips.SelectMany(trip => trip.FlightSegments)
                        .Any(segment => request.IncludedAirlines.Contains(segment.AirlineCode)));
        }

        private static IEnumerable<FlightItineraryApi> FilterByDepartureTimeSpan(IEnumerable<FlightItineraryApi> filteredItins, FlightFilterApiRequest request)
        {
            return filteredItins.Where(
                itin =>
                    itin.FlightTrips.SelectMany(trip => trip.FlightSegments)
                        .Any(segment => request.DepartureTimeSpans.TrueForAll(span =>
                            segment.DepartureTime >= span.Start && segment.DepartureTime <= span.End)));
        }

        private static IEnumerable<FlightItineraryApi> FilterByArrivalTimeSpan(IEnumerable<FlightItineraryApi> filteredItins, FlightFilterApiRequest request)
        {
            return filteredItins.Where(
                itin =>
                    itin.FlightTrips.SelectMany(trip => trip.FlightSegments)
                        .Any(segment => request.ArrivalTimeSpans.TrueForAll(span =>
                            segment.ArrivalTime >= span.Start && segment.ArrivalTime <= span.End)));
        }

        private static IEnumerable<FlightItineraryApi> FilterByTotalTransit(IEnumerable<FlightItineraryApi> filteredItins, FlightFilterApiRequest request)
        {
            return filteredItins.Where(itin => itin.FlightTrips.Any(trip => request.TotalTransits.Contains(trip.TotalTransit)));
        }

        private static IEnumerable<FlightItineraryApi> GetUnfilteredItineraries(FlightFilterApiRequest request)
        {
            var service = FlightService.GetInstance();
            var rawItins = service.GetItinerariesFromCache(request.SearchId);
            var unfilteredItineraries = service.ConvertToItinerariesApi(rawItins);
            return unfilteredItineraries;
        }

        private static FlightFilterApiResponse AssembleApiResponse(List<FlightItineraryApi> filteredItinList, FlightFilterApiRequest request)
        {
            return new FlightFilterApiResponse
            {
                TotalFlightCount = filteredItinList.Count(),
                FlightList = filteredItinList,
                OriginalRequest = request
            };
        }
    }
}