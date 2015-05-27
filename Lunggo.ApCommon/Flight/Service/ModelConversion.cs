using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Config;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public List<FlightItineraryApi> ConvertToItinerariesApi(IEnumerable<FlightItineraryFare> itineraries)
        {
            var list = itineraries.Select(ConvertToItineraryApi).ToList();
            for (var i = 0; i < list.Count; i++)
            {
                list[i].SequenceNo = i;
            }
            var orderedList = list.OrderBy(itin => itin.SequenceNo);
            return orderedList.ToList();
        }

        public FlightItineraryApi ConvertToItineraryApi(FlightItineraryFare itinerary)
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
                TotalFare = itinerary.IdrPrice,
                FlightTrips = MapTrips(itinerary.FlightTrips),
            };
        }

        internal FlightTripApi ConvertToTripApi(FlightTripTableRecord summaryRecord)
        {
            var dict = DictionaryService.GetInstance();
            return new FlightTripApi
            {
                OriginAirport = summaryRecord.OriginAirportCd,
                OriginCity = dict.GetAirportCity(summaryRecord.OriginAirportCd),
                DestinationAirport = summaryRecord.DestinationAirportCd,
                DestinationCity = dict.GetAirportCity(summaryRecord.DestinationAirportCd)
            };
        }

        internal FlightSegmentApi ConvertToSegmentApi(FlightSegmentTableRecord summaryRecord)
        {
            var dict = DictionaryService.GetInstance();
            return new FlightSegmentApi
            {
                DepartureAirport = summaryRecord.DepartureAirportCd,
                DepartureCity = dict.GetAirportCity(summaryRecord.DepartureAirportCd),
                DepartureTime = summaryRecord.DepartureTime.GetValueOrDefault(),
                ArrivalAirport = summaryRecord.ArrivalAirportCd,
                ArrivalCity = dict.GetAirportCity(summaryRecord.ArrivalAirportCd),
                ArrivalTime = summaryRecord.ArrivalTime.GetValueOrDefault(),
                AirlineCode = summaryRecord.AirlineCd,
                FlightNumber = summaryRecord.FlightNumber
            };
        }

        internal PassengerInfoFare ConvertToPassengerApi(FlightPassengerTableRecord summaryRecord)
        {
            return new PassengerInfoFare
            {
                Title = TitleCd.Mnemonic(summaryRecord.TitleCd),
                FirstName = summaryRecord.FirstName,
                LastName = summaryRecord.LastName,
                Type = PassengerTypeCd.Mnemonic(summaryRecord.PassengerTypeCd)
            };
        }

        internal PaymentInfo ConvertFlightPaymentInfo(FlightReservationTableRecord record)
        {
            return new PaymentInfo
            {
                Method = PaymentMethodCd.Mnemonic(record.PaymentMethodCd),
                Status = PaymentStatusCd.Mnemonic(record.PaymentStatusCd),
                TargetAccount = record.PaymentTargetAccount
            };
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
            var airlineLogoPath = ConfigManager.GetInstance().GetConfigValue("general", "airlineLogoRootUrl");
            var airlineLogoExtension = ConfigManager.GetInstance().GetConfigValue("general", "airlineLogoExtension");
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
                AirlineName = dict.GetAirlineName(segment.AirlineCode),
                AirlineLogoUrl = airlineLogoPath + segment.AirlineCode + airlineLogoExtension,
                FlightNumber = segment.FlightNumber,
                OperatingAirlineCode = segment.OperatingAirlineCode,
                OperatingAirlineName = dict.GetAirlineName(segment.OperatingAirlineCode),
                OperatingAirlineLogoUrl = airlineLogoPath + segment.OperatingAirlineCode + airlineLogoExtension,
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
            var airlineLogoPath = ConfigManager.GetInstance().GetConfigValue("general", "airlineLogoRootUrl");
            var airlineLogoExtension = ConfigManager.GetInstance().GetConfigValue("general", "airlineLogoExtension");
            var segments = trip.FlightSegments;
            var airlineCodes = segments.Select(segment => segment.AirlineCode);
            var airlines = airlineCodes.Distinct().Select(code => new Airline
            {
                Code = code,
                Name = dict.GetAirlineName(code),
                LogoUrl = airlineLogoPath + code + airlineLogoExtension
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
    }
}
