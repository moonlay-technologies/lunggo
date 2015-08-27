using System;
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
        internal FlightReservationApi ConvertToReservationApi(FlightReservation reservation)
        {
            if (reservation != null)
            {
                return new FlightReservationApi
                {
                    RsvNo = reservation.RsvNo,
                    RsvTime = reservation.RsvTime,
                    TripType = reservation.TripType,
                    InvoiceNo = reservation.InvoiceNo,
                    Itinerary = ConvertToItineraryApi(BundleItineraries(reservation.Itineraries)),
                    Contact = reservation.Contact,
                    Passengers = reservation.Passengers,
                    Payment = reservation.Payment
                };
            }
            else
            {
                return new FlightReservationApi();
            }
        }

        internal FlightItineraryApi ConvertToItineraryApi(FlightItinerary itinerary)
        {
            if (itinerary != null)
            {
                return new FlightItineraryApi
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
                    FlightTrips = MapTrips(itinerary.FlightTrips),
                };
            }
            else
            {
                return new FlightItineraryApi();
            }
        }

        internal FlightTripApi ConvertToTripApi(FlightTripTableRecord summaryRecord)
        {
            var dict = DictionaryService.GetInstance();
            return new FlightTripApi
            {
                OriginAirport = summaryRecord.OriginAirportCd,
                OriginCity = dict.GetAirportCity(summaryRecord.OriginAirportCd),
                DestinationAirport = summaryRecord.DestinationAirportCd,
                DestinationCity = dict.GetAirportCity(summaryRecord.DestinationAirportCd),
            };
        }

        internal FlightTrip ConvertToTripDetails(FlightTripTableRecord summaryRecord)
        {
            var dict = DictionaryService.GetInstance();
            return new FlightTrip
            {
                OriginAirport = summaryRecord.OriginAirportCd,
                OriginAirportName = dict.GetAirportName(summaryRecord.OriginAirportCd),
                OriginCity = dict.GetAirportCity(summaryRecord.OriginAirportCd),
                DestinationAirport = summaryRecord.DestinationAirportCd,
                DestinationAirportName = dict.GetAirportName(summaryRecord.DestinationAirportCd),
                DestinationCity = dict.GetAirportCity(summaryRecord.DestinationAirportCd),
                DepartureDate = summaryRecord.DepartureDate.GetValueOrDefault().ToUniversalTime(),
            };
        }

        internal FlightSegment ConvertToSegmentApi(FlightSegmentTableRecord summaryRecord)
        {
            var dict = DictionaryService.GetInstance();
            return new FlightSegment
            {
                DepartureAirport = summaryRecord.DepartureAirportCd,
                DepartureCity = dict.GetAirportCity(summaryRecord.DepartureAirportCd),
                DepartureTime = summaryRecord.DepartureTime.GetValueOrDefault().ToUniversalTime(),
                ArrivalAirport = summaryRecord.ArrivalAirportCd,
                ArrivalCity = dict.GetAirportCity(summaryRecord.ArrivalAirportCd),
                ArrivalTime = summaryRecord.ArrivalTime.GetValueOrDefault().ToUniversalTime(),
                AirlineCode = summaryRecord.AirlineCd,
                FlightNumber = summaryRecord.FlightNumber
            };
        }

        internal FlightSegment ConvertToSegmentDetails(FlightSegmentTableRecord summaryRecord)
        {
            var dict = DictionaryService.GetInstance();
            return new FlightSegment
            {
                DepartureAirport = summaryRecord.DepartureAirportCd,
                DepartureAirportName = dict.GetAirportName(summaryRecord.DepartureAirportCd),
                DepartureCity = dict.GetAirportCity(summaryRecord.DepartureAirportCd),
                DepartureTerminal = summaryRecord.DepartureTerminal,
                DepartureTime = summaryRecord.DepartureTime.GetValueOrDefault().ToUniversalTime(),
                ArrivalAirport = summaryRecord.ArrivalAirportCd,
                ArrivalAirportName = dict.GetAirportName(summaryRecord.ArrivalAirportCd),
                ArrivalCity = dict.GetAirportCity(summaryRecord.ArrivalAirportCd),
                ArrivalTerminal = summaryRecord.ArrivalTerminal,
                ArrivalTime = summaryRecord.ArrivalTime.GetValueOrDefault().ToUniversalTime(),
                AirlineCode = summaryRecord.AirlineCd,
                AirlineName = dict.GetAirlineName(summaryRecord.AirlineCd),
                AirlineLogoUrl = dict.GetAirlineLogoUrl(summaryRecord.AirlineCd),
                OperatingAirlineCode = summaryRecord.OperatingAirlineCd,
                OperatingAirlineName = dict.GetAirlineName(summaryRecord.OperatingAirlineCd),
                OperatingAirlineLogoUrl = dict.GetAirlineLogoUrl(summaryRecord.OperatingAirlineCd),
                AircraftCode = summaryRecord.AircraftCd,
                FlightNumber = summaryRecord.FlightNumber,
                Baggage = summaryRecord.Baggage,
                Duration = summaryRecord.Duration.GetValueOrDefault(),
            };
        }

        internal FlightPassenger ConvertToPassengerApi(FlightPassengerTableRecord summaryRecord)
        {
            return new FlightPassenger
            {
                Title = TitleCd.Mnemonic(summaryRecord.TitleCd),
                FirstName = summaryRecord.FirstName,
                LastName = summaryRecord.LastName,
                Type = PassengerTypeCd.Mnemonic(summaryRecord.PassengerTypeCd)
            };
        }

        internal FlightPassenger ConvertToPassengerDetails(FlightPassengerTableRecord summaryRecord)
        {
            return new FlightPassenger
            {
                Title = TitleCd.Mnemonic(summaryRecord.TitleCd),
                FirstName = summaryRecord.FirstName,
                LastName = summaryRecord.LastName,
                Type = PassengerTypeCd.Mnemonic(summaryRecord.PassengerTypeCd)
            };
        }

        internal PaymentInfo ConvertFlightPaymentInfo(FlightReservationTableRecord record)
        {
            DateTime? paymentTime = null;
            if (record.PaymentTime.HasValue)
                paymentTime = record.PaymentTime.Value.ToUniversalTime();
            RefundInfo refund = null;
            if (record.RefundTime != null)
                refund = new RefundInfo
                {
                    Time = record.RefundTime.GetValueOrDefault(),
                    Amount = record.RefundAmount.GetValueOrDefault(),
                    TargetBank = record.RefundTargetBank,
                    TargetAccount = record.RefundTargetAccount
                };
            return new PaymentInfo
            {
                Id = record.PaymentId,
                Medium = PaymentMediumCd.Mnemonic(record.PaymentMediumCd),
                Method = PaymentMethodCd.Mnemonic(record.PaymentMethodCd),
                Status = PaymentStatusCd.Mnemonic(record.PaymentStatusCd),
                Time = paymentTime,
                TargetAccount = record.PaymentTargetAccount,
                FinalPrice = record.FinalPrice.GetValueOrDefault(),
                PaidAmount = record.PaidAmount.GetValueOrDefault(),
                Refund = refund
            };
        }

        private static List<FlightTripApi> MapTrips(IEnumerable<FlightTrip> trips)
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
                DepartureDate = trip.DepartureDate.ToUniversalTime(),
                TotalDuration = CalculateTotalDuration(trip),
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
                DepartureTime = segment.DepartureTime.ToUniversalTime(),
                ArrivalAirport = segment.ArrivalAirport,
                ArrivalCity = dict.GetAirportCity(segment.ArrivalAirport),
                ArrivalAirportName = dict.GetAirportName(segment.ArrivalAirport),
                ArrivalTime = segment.ArrivalTime.ToUniversalTime(),
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
                Rbd = segment.Rbd,
                Meal = segment.Meal,
                RemainingSeats = segment.RemainingSeats
            }).ToList();
        }

        private static TimeSpan CalculateTotalDuration(FlightTrip trip)
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

        private static List<Airline> GetAirlineList(FlightTrip trip)
        {
            var dict = DictionaryService.GetInstance();
            var segments = trip.FlightSegments;
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
                        ArrivalTime = stop.ArrivalTime.ToUniversalTime(),
                        DepartureTime = stop.DepartureTime.ToUniversalTime(),
                        Duration = stop.DepartureTime - stop.ArrivalTime
                    }));
                }
                if (i != 0)
                {
                    result.Add(new Transit
                    {
                        IsStop = false,
                        Airport = segments[i].DepartureAirport,
                        ArrivalTime = segments[i - 1].ArrivalTime.ToUniversalTime(),
                        DepartureTime = segments[i].DepartureTime.ToUniversalTime(),
                        Duration = segments[i].DepartureTime - segments[i - 1].ArrivalTime
                    });
                }
            }
            return result;
        }

        private static int CalculateTotalTransit(FlightTrip trip)
        {
            var segments = trip.FlightSegments;
            var transit = segments.Count() - 1;
            var stop = segments.Sum(segment => segment.StopQuantity);
            return transit + stop;
        }
    }
}
