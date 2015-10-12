﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Fizzler;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Config;
using Lunggo.Repository.TableRecord;
using Newtonsoft.Json.Serialization;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        internal FlightReservationForDisplay ConvertToReservationForDisplay(FlightReservation reservation)
        {
            if (reservation != null)
            {
                return new FlightReservationForDisplay
                {
                    RsvNo = reservation.RsvNo,
                    RsvTime = reservation.RsvTime,
                    IsIssued = reservation.Itineraries.TrueForAll(itin => itin.BookingStatus == BookingStatus.Ticketed),
                    TripType = reservation.TripType,
                    InvoiceNo = reservation.InvoiceNo,
                    Itinerary = ConvertToItineraryForDisplay(BundleItineraries(reservation.Itineraries)),
                    Contact = reservation.Contact,
                    Passengers = reservation.Passengers,
                    Payment = reservation.Payment
                };
            }
            else
            {
                return new FlightReservationForDisplay();
            }
        }

        internal FlightItineraryForDisplay ConvertToItineraryForDisplay(FlightItinerary itinerary)
        {
            if (itinerary != null)
            {
                return new FlightItineraryForDisplay
                {
                    AdultCount = itinerary.AdultCount,
                    ChildCount = itinerary.ChildCount,
                    InfantCount = itinerary.InfantCount,
                    RequireBirthDate = itinerary.RequireBirthDate,
                    RequirePassport = itinerary.RequirePassport,
                    RequireSameCheckIn = itinerary.RequireSameCheckIn,
                    CanHold = itinerary.CanHold,
                    TripType = itinerary.TripType,
                    RequestedCabinClass = itinerary.RequestedCabinClass,
                    TotalFare = itinerary.LocalPrice,
                    Trips = MapTrips(itinerary.Trips)
                };
            }
            else
            {
                return new FlightItineraryForDisplay();
            }
        }

        private static List<FlightTripForDisplay> MapTrips(IEnumerable<FlightTrip> trips)
        {
            var dict = DictionaryService.GetInstance();
            return trips.Select(trip => new FlightTripForDisplay
            {
                Segments = MapSegments(trip.Segments),
                OriginAirport = trip.OriginAirport,
                OriginCity = dict.GetAirportCity(trip.OriginAirport),
                OriginAirportName = dict.GetAirportName(trip.OriginAirport),
                DestinationAirport = trip.DestinationAirport,
                DestinationCity = dict.GetAirportCity(trip.DestinationAirport),
                DestinationAirportName = dict.GetAirportName(trip.DestinationAirport),
                DepartureDate = trip.DepartureDate,
                TotalDuration = CalculateTotalDuration(trip).TotalMilliseconds,
                Airlines = GetAirlineList(trip),
                TotalTransit = CalculateTotalTransit(trip),
                Transits = MapTransitDetails(trip)
            }).ToList();
        }

        private static List<FlightSegment> MapSegments(IEnumerable<FlightSegment> segments)
        {
            var dict = DictionaryService.GetInstance();
            return segments.Select(segment => new FlightSegment
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
                AirlineName = dict.GetAirlineName(segment.AirlineCode),
                AirlineLogoUrl = dict.GetAirlineLogoUrl(segment.AirlineCode),
                FlightNumber = segment.FlightNumber,
                OperatingAirlineCode = segment.OperatingAirlineCode,
                OperatingAirlineName = dict.GetAirlineName(segment.OperatingAirlineCode),
                OperatingAirlineLogoUrl = dict.GetAirlineLogoUrl(segment.OperatingAirlineCode),
                AircraftCode = segment.AircraftCode,
                StopQuantity = segment.StopQuantity,
                FlightStops = segment.FlightStops,
                Pnr = segment.Pnr,
                Rbd = segment.Rbd,
                Meal = segment.Meal,
                RemainingSeats = segment.RemainingSeats
            }).ToList();
        }

        private static TimeSpan CalculateTotalDuration(FlightTrip trip)
        {
            var segments = trip.Segments;
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

        private static List<Airline> GetAirlineList(FlightTrip trip)
        {
            var dict = DictionaryService.GetInstance();
            var segments = trip.Segments;
            var airlineCodes = segments.Select(segment => segment.AirlineCode);
            var airlines = airlineCodes.Distinct().Select(code => new Airline
            {
                Code = code,
                Name = dict.GetAirlineName(code),
                LogoUrl = dict.GetAirlineLogoUrl(code)
            });
            return airlines.ToList();
        }

        private static List<Transit> MapTransitDetails(FlightTrip trip)
        {
            var segments = trip.Segments;
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
                        Duration = stop.DepartureTime - stop.ArrivalTime
                    }));
                }
                if (i != 0)
                {
                    result.Add(new Transit
                    {
                        IsStop = false,
                        Airport = segments[i].DepartureAirport,
                        ArrivalTime = segments[i - 1].ArrivalTime,
                        DepartureTime = segments[i].DepartureTime,
                        Duration = segments[i].DepartureTime - segments[i - 1].ArrivalTime
                    });
                }
            }
            return result;
        }

        private static int CalculateTotalTransit(FlightTrip trip)
        {
            var segments = trip.Segments;
            var transit = segments.Count() - 1;
            var stop = segments.Sum(segment => segment.StopQuantity);
            return transit + stop;
        }
    }
}
