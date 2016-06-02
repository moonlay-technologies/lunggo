using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Service;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        internal FlightReservationForDisplay ConvertToReservationForDisplay(FlightReservation reservation)
        {
            if (reservation == null)
                return null;

            return new FlightReservationForDisplay
            {
                RsvNo = reservation.RsvNo,
                RsvTime = reservation.RsvTime,
                RsvStatus = reservation.RsvStatus,
                CancellationType = reservation.CancellationType,
                CancellationTime = reservation.CancellationTime,
                OverallTripType = reservation.OverallTripType,
                Itinerary = ConvertToItineraryForDisplay(reservation.Itineraries),
                Contact = reservation.Contact,
                Passengers = reservation.Passengers,
                Payment = PaymentService.GetInstance().ConvertToPaymentDetailsForDisplay(reservation.Payment),
                User = reservation.User
            };
        }

        internal FlightItineraryForDisplay ConvertToItineraryForDisplay(List<FlightItinerary> itins)
        {
            if (itins == null || itins.Count == 0)
                return null;

            var totalFare = itins.Sum(itin => itin.Price.Local);
            var tripType =
                ParseTripType(
                    itins.SelectMany(itin => itin.Trips).OrderBy(trip => trip.Segments[0].DepartureTime).ToList());
            return new FlightItineraryForDisplay
            {
                AdultCount = itins[0].AdultCount,
                ChildCount = itins[0].ChildCount,
                InfantCount = itins[0].InfantCount,
                RequireBirthDate = itins.Exists(itin => itin.RequireBirthDate),
                RequirePassport = itins.Exists(itin => itin.RequirePassport),
                RequireSameCheckIn = itins.Exists(itin => itin.RequireSameCheckIn),
                RequireNationality = itins.Exists(itin => itin.RequireNationality),
                RequestedCabinClass = itins[0].RequestedCabinClass,
                CanHold = itins.TrueForAll(itin => itin.CanHold),
                Currency = itins[0].Price.LocalCurrency,
                TripType = tripType,
                TotalFare = totalFare,
                Trips = itins[0].Trips.Select(ConvertToTripForDisplay).ToList(),
                RegisterNumber = 0,
                OriginalFare = GenerateDummyOriginalFare(totalFare)
            };
        }

        internal FlightItineraryForDisplay ConvertToItineraryForDisplay(FlightItinerary itinerary)
        {
            if (itinerary == null)
                return null;

            return new FlightItineraryForDisplay
            {
                AdultCount = itinerary.AdultCount,
                ChildCount = itinerary.ChildCount,
                InfantCount = itinerary.InfantCount,
                RequireBirthDate = itinerary.RequireBirthDate,
                RequirePassport = itinerary.RequirePassport,
                RequireSameCheckIn = itinerary.RequireSameCheckIn,
                RequireNationality = itinerary.RequireNationality,
                RequestedCabinClass = itinerary.RequestedCabinClass,
                CanHold = itinerary.CanHold,
                Currency = itinerary.Price.LocalCurrency,
                TripType = itinerary.TripType,
                TotalFare = itinerary.Price.Local,
                Trips = itinerary.Trips.Select(ConvertToTripForDisplay).ToList(),
                RegisterNumber = itinerary.RegisterNumber,
                OriginalFare = GenerateDummyOriginalFare(itinerary.Price.Local)
            };
        }

        private static ComboForDisplay ConvertToComboForDisplay(Combo combo)
        {
            if (combo == null)
                return null;
            return new ComboForDisplay
            {
                Fare = combo.Fare,
                Registers = combo.Registers
            };
        }

        private FlightTripForDisplay ConvertToTripForDisplay(FlightTrip trip)
        {
            return new FlightTripForDisplay
            {
                Segments = MapSegments(trip.Segments),
                OriginAirport = trip.OriginAirport,
                OriginCity = GetAirportCity(trip.OriginAirport),
                OriginAirportName = GetAirportName(trip.OriginAirport),
                DestinationAirport = trip.DestinationAirport,
                DestinationCity = GetAirportCity(trip.DestinationAirport),
                DestinationAirportName = GetAirportName(trip.DestinationAirport),
                DepartureDate = trip.DepartureDate,
                TotalDuration = CalculateTotalDuration(trip).TotalMilliseconds,
                Airlines = GetAirlineList(trip),
                TotalTransit = CalculateTotalTransit(trip),
                Transits = MapTransitDetails(trip)
            };
        }

        private List<FlightSegmentForDisplay> MapSegments(IEnumerable<FlightSegment> segments)
        {
            return segments.Select(segment => new FlightSegmentForDisplay
            {
                DepartureAirport = segment.DepartureAirport,
                DepartureCity = GetAirportCity(segment.DepartureAirport),
                DepartureAirportName = GetAirportName(segment.DepartureAirport),
                DepartureTime = segment.DepartureTime,
                DepartureTerminal = segment.DepartureTerminal,
                ArrivalAirport = segment.ArrivalAirport,
                ArrivalCity = GetAirportCity(segment.ArrivalAirport),
                ArrivalAirportName = GetAirportName(segment.ArrivalAirport),
                ArrivalTime = segment.ArrivalTime,
                ArrivalTerminal = segment.ArrivalTerminal,
                Duration = segment.Duration.TotalMilliseconds,
                AirlineCode = segment.AirlineCode,
                AirlineName = GetAirlineName(segment.AirlineCode),
                AirlineLogoUrl = GetAirlineLogoUrl(segment.AirlineCode),
                FlightNumber = segment.FlightNumber,
                OperatingAirlineCode = segment.OperatingAirlineCode,
                OperatingAirlineName = GetAirlineName(segment.OperatingAirlineCode),
                OperatingAirlineLogoUrl = GetAirlineLogoUrl(segment.OperatingAirlineCode),
                AircraftCode = segment.AircraftCode,
                StopQuantity = segment.StopQuantity,
                Stops = MapStops(segment.Stops),
                CabinClass = segment.CabinClass,
                Pnr = segment.Pnr,
                Rbd = segment.Rbd,
                IsMealIncluded = segment.IsMealIncluded,
                IsPscIncluded = segment.IsPscIncluded,
                Baggage = segment.Baggage,
                RemainingSeats = segment.RemainingSeats,
            }).ToList();
        }

        private List<FlightStopForDisplay> MapStops(IEnumerable<FlightStop> stops)
        {
            if (stops == null)
                return null;

            return stops.Select(stop => new FlightStopForDisplay
            {
                Airport = stop.Airport,
                ArrivalTime = stop.ArrivalTime,
                DepartureTime = stop.DepartureTime,
                Duration = stop.Duration.Milliseconds
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

        private List<Airline> GetAirlineList(FlightTrip trip)
        {
            var segments = trip.Segments;
            var airlineCodes = segments.Select(segment => segment.AirlineCode);
            var airlines = airlineCodes.Distinct().Select(code => new Airline
            {
                Code = code,
                Name = GetAirlineName(code),
                LogoUrl = GetAirlineLogoUrl(code)
            });
            return airlines.ToList();
        }

        private static List<Transit> MapTransitDetails(FlightTrip trip)
        {
            var segments = trip.Segments;
            var result = new List<Transit>();
            for (var i = 0; i < segments.Count; i++)
            {
                if (segments[i].Stops != null && segments[i].Stops.Any())
                {
                    result.AddRange(segments[i].Stops.Select(stop => new Transit
                    {
                        IsStop = true,
                        Airport = stop.Airport,
                        ArrivalTime = stop.ArrivalTime,
                        DepartureTime = stop.DepartureTime,
                        Duration = (stop.DepartureTime - stop.ArrivalTime).TotalMilliseconds
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
                        Duration = (segments[i].DepartureTime - segments[i - 1].ArrivalTime).TotalMilliseconds
                    });
                }
            }
            return result;
        }

        private static int CalculateTotalTransit(FlightTrip trip)
        {
            var segments = trip.Segments;
            var transit = segments.Count() - 1;
            return transit;
        }

        private static decimal GenerateDummyOriginalFare(decimal fare)
        {
            var markup = (((fare / 100M) % 200M) + 50M) / 10000M; // 0.5% ~ 2.5%
            var unroundedFare = fare * (1M + markup);
            return unroundedFare - (unroundedFare % 100M);
        }
    }
}
