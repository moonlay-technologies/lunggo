using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Flight.Utility;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Database.Logic
{
    internal class GetFlightDb
    {
        internal static FlightItineraryApi Summary(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var service = FlightService.GetInstance();
                var tripRecords = GetFlightTripSummaryQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).ToList();
                return tripRecords.Any()
                    ? new FlightItineraryApi
                    {
                        FlightTrips = tripRecords.Select(tripRecord =>
                        {
                            var segmentRecords = GetFlightSegmentSummaryQuery.GetInstance()
                                .Execute(conn, new { tripRecord.TripId });
                            var tripSummary = service.ConvertToTripApi(tripRecord);
                            tripSummary.FlightSegments =
                                segmentRecords.Select(service.ConvertToSegmentApi).ToList();
                            return tripSummary;
                        }).ToList(),
                        TotalFare = GetFlightTotalFareQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).Sum(),
                        Currency = GetFlightLocalCurrencyQuery.GetInstance().Execute(conn, new { RsvNo = rsvNo }).Single()
                    }
                    : null;
            }
        }

        internal static IEnumerable<FlightReservation> OverviewReservations(string contactEmail)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var rsvNos = GetFlightRsvNosByContactEmailQuery.GetInstance().Execute(conn, new { ContactEmail = contactEmail });
                foreach (var rsvNo in rsvNos)
                {
                    yield return OverviewReservation(rsvNo);
                }
            }
        }

        internal static FlightReservation OverviewReservation(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                FlightReservation reservation = null;
                var itineraryLookup = new Dictionary<long, FlightItineraryDetails>();
                var tripLookup = new Dictionary<long, FlightTripDetails>();
                var segmentLookup = new Dictionary<long, FlightSegmentDetails>();
                var passengerLookup = new Dictionary<long, PassengerInfoDetails>();
                var dict = DictionaryService.GetInstance();
                reservation = GetFlightReservationQuery.GetInstance().ExecuteMultiMap(conn, new { RsvNo = rsvNo },
                    (reservationRecord, itineraryRecord, tripRecord, segmentRecord, passengerRecord) =>
                    {
                        if (reservation == null)
                        {
                            reservation = new FlightReservation
                            {
                                RsvNo = rsvNo,
                                InvoiceNo = reservationRecord.InvoiceNo,
                                ContactData = new ContactData
                                {
                                    Name = reservationRecord.ContactName,
                                    Email = reservationRecord.ContactEmail,
                                    CountryCode = reservationRecord.ContactCountryCd,
                                    Phone = reservationRecord.ContactPhone
                                },
                                PaymentInfo = new PaymentInfo
                                {
                                    Time = reservationRecord.PaymentTime
                                },
                                TripType = TripTypeCd.Mnemonic(reservationRecord.OverallTripTypeCd),
                                Itinerary = new FlightItineraryDetails(),
                                Passengers = new List<PassengerInfoDetails>()
                            };
                        }
                        FlightItineraryDetails itinerary;
                        if (!itineraryLookup.TryGetValue(itineraryRecord.ItineraryId.GetValueOrDefault(), out itinerary))
                        {
                            itinerary = new FlightItineraryDetails
                            {
                                FlightTrips = new List<FlightTripDetails>()
                            };
                            reservation.Itinerary = itinerary;
                            itineraryLookup.Add(itineraryRecord.ItineraryId.GetValueOrDefault(), itinerary);
                        }
                        FlightTripDetails trip;
                        if (!tripLookup.TryGetValue(tripRecord.TripId.GetValueOrDefault(), out trip))
                        {
                            trip = new FlightTripDetails
                            {
                                OriginAirport = tripRecord.OriginAirportCd,
                                OriginAirportName = dict.GetAirportName(tripRecord.OriginAirportCd),
                                OriginCity = dict.GetAirportCity(tripRecord.OriginAirportCd),
                                DestinationAirport = tripRecord.DestinationAirportCd,
                                DestinationAirportName = dict.GetAirportName(tripRecord.DestinationAirportCd),
                                DestinationCity = dict.GetAirportCity(tripRecord.DestinationAirportCd),
                                FlightSegments = new List<FlightSegmentDetails>()
                            };
                            tripLookup.Add(tripRecord.TripId.GetValueOrDefault(), trip);
                            itinerary.FlightTrips.Add(trip);
                        }
                        FlightSegmentDetails segment;
                        if (!segmentLookup.TryGetValue(segmentRecord.SegmentId.GetValueOrDefault(), out segment))
                        {
                            segment = new FlightSegmentDetails
                            {
                                Pnr = segmentRecord.Pnr,
                                OperatingAirlineCode = segmentRecord.OperatingAirlineCd,
                                OperatingAirlineName = dict.GetAirlineName(segmentRecord.OperatingAirlineCd),
                                OperatingAirlineLogoUrl = dict.GetAirlineLogoUrl(segmentRecord.OperatingAirlineCd),
                                AirlineCode = segmentRecord.AirlineCd,
                                AirlineName = dict.GetAirlineName(segmentRecord.AirlineCd),
                                AirlineLogoUrl = dict.GetAirlineLogoUrl(segmentRecord.AirlineCd),
                                FlightNumber = segmentRecord.FlightNumber,
                                DepartureAirport = segmentRecord.DepartureAirportCd,
                                DepartureAirportName = dict.GetAirportName(segmentRecord.DepartureAirportCd),
                                DepartureCity = dict.GetAirportCity(segmentRecord.DepartureAirportCd),
                                DepartureTerminal = segmentRecord.DepartureTerminal,
                                DepartureTime = segmentRecord.DepartureTime.GetValueOrDefault(),
                                ArrivalAirport = segmentRecord.ArrivalAirportCd,
                                ArrivalAirportName = dict.GetAirportName(segmentRecord.ArrivalAirportCd),
                                ArrivalCity = dict.GetAirportCity(segmentRecord.ArrivalAirportCd),
                                ArrivalTerminal = segmentRecord.ArrivalTerminal,
                                ArrivalTime = segmentRecord.ArrivalTime.GetValueOrDefault(),
                                Baggage = segmentRecord.Baggage
                            };
                            segmentLookup.Add(segmentRecord.SegmentId.GetValueOrDefault(), segment);
                            trip.FlightSegments.Add(segment);
                        }
                        PassengerInfoDetails passenger;
                        if (!passengerLookup.TryGetValue(passengerRecord.PassengerId.GetValueOrDefault(), out passenger))
                        {
                            passenger = new PassengerInfoDetails
                            {
                                Title = TitleCd.Mnemonic(passengerRecord.TitleCd),
                                FirstName = passengerRecord.FirstName,
                                LastName = passengerRecord.LastName,
                                Type = PassengerTypeCd.Mnemonic(passengerRecord.PassengerTypeCd)
                            };
                            reservation.Passengers.Add(passenger);
                            passengerLookup.Add(passengerRecord.PassengerId.GetValueOrDefault(), passenger);
                        }
                        return reservation;
                    }, "ItineraryId,TripId,SegmentId,PassengerId").Distinct().Single();
                return reservation;
            }
        }

        internal static FlightReservation Reservation(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                FlightReservation reservation = null;
                var itineraryLookup = new Dictionary<long, FlightItineraryDetails>();
                var tripLookup = new Dictionary<long, FlightTripDetails>();
                var segmentLookup = new Dictionary<long, FlightSegmentDetails>();
                var passengerLookup = new Dictionary<long, PassengerInfoDetails>();
                var dict = DictionaryService.GetInstance();
                reservation = GetFlightReservationQuery.GetInstance().ExecuteMultiMap(conn, new { RsvNo = rsvNo },
                    (reservationRecord, itineraryRecord, tripRecord, segmentRecord, passengerRecord) =>
                    {
                        if (reservation == null)
                        {
                            reservation = new FlightReservation
                            {
                                RsvNo = rsvNo,
                                InvoiceNo = reservationRecord.InvoiceNo,
                                ContactData = new ContactData
                                {
                                    Name = reservationRecord.ContactName,
                                    Email = reservationRecord.ContactEmail,
                                    CountryCode = reservationRecord.ContactCountryCd,
                                    Phone = reservationRecord.ContactPhone
                                },
                                PaymentInfo = new PaymentInfo
                                {
                                    Time = reservationRecord.PaymentTime
                                },
                                TripType = TripTypeCd.Mnemonic(reservationRecord.OverallTripTypeCd),
                                Itinerary = new FlightItineraryDetails(),
                                Passengers = new List<PassengerInfoDetails>()
                            };
                        }
                        FlightItineraryDetails itinerary;
                        if (!itineraryLookup.TryGetValue(itineraryRecord.ItineraryId.GetValueOrDefault(), out itinerary))
                        {
                            itinerary = new FlightItineraryDetails
                            {
                                FlightTrips = new List<FlightTripDetails>()
                            };
                            reservation.Itinerary = itinerary;
                            itineraryLookup.Add(itineraryRecord.ItineraryId.GetValueOrDefault(), itinerary);
                        }
                        FlightTripDetails trip;
                        if (!tripLookup.TryGetValue(tripRecord.TripId.GetValueOrDefault(), out trip))
                        {
                            trip = new FlightTripDetails
                            {
                                OriginAirport = tripRecord.OriginAirportCd,
                                OriginAirportName = dict.GetAirportName(tripRecord.OriginAirportCd),
                                OriginCity = dict.GetAirportCity(tripRecord.OriginAirportCd),
                                DestinationAirport = tripRecord.DestinationAirportCd,
                                DestinationAirportName = dict.GetAirportName(tripRecord.DestinationAirportCd),
                                DestinationCity = dict.GetAirportCity(tripRecord.DestinationAirportCd),
                                FlightSegments = new List<FlightSegmentDetails>()
                            };
                            tripLookup.Add(tripRecord.TripId.GetValueOrDefault(), trip);
                            itinerary.FlightTrips.Add(trip);
                        }
                        FlightSegmentDetails segment;
                        if (!segmentLookup.TryGetValue(segmentRecord.SegmentId.GetValueOrDefault(), out segment))
                        {
                            segment = new FlightSegmentDetails
                            {
                                Pnr = segmentRecord.Pnr,
                                OperatingAirlineCode = segmentRecord.OperatingAirlineCd,
                                OperatingAirlineName = dict.GetAirlineName(segmentRecord.OperatingAirlineCd),
                                OperatingAirlineLogoUrl = dict.GetAirlineLogoUrl(segmentRecord.OperatingAirlineCd),
                                AirlineCode = segmentRecord.AirlineCd,
                                AirlineName = dict.GetAirlineName(segmentRecord.AirlineCd),
                                AirlineLogoUrl = dict.GetAirlineLogoUrl(segmentRecord.AirlineCd),
                                FlightNumber = segmentRecord.FlightNumber,
                                DepartureAirport = segmentRecord.DepartureAirportCd,
                                DepartureAirportName = dict.GetAirportName(segmentRecord.DepartureAirportCd),
                                DepartureCity = dict.GetAirportCity(segmentRecord.DepartureAirportCd),
                                DepartureTerminal = segmentRecord.DepartureTerminal,
                                DepartureTime = segmentRecord.DepartureTime.GetValueOrDefault(),
                                ArrivalAirport = segmentRecord.ArrivalAirportCd,
                                ArrivalAirportName = dict.GetAirportName(segmentRecord.ArrivalAirportCd),
                                ArrivalCity = dict.GetAirportCity(segmentRecord.ArrivalAirportCd),
                                ArrivalTerminal = segmentRecord.ArrivalTerminal,
                                ArrivalTime = segmentRecord.ArrivalTime.GetValueOrDefault(),
                                Baggage = segmentRecord.Baggage
                            };
                            segmentLookup.Add(segmentRecord.SegmentId.GetValueOrDefault(), segment);
                            trip.FlightSegments.Add(segment);
                        }
                        PassengerInfoDetails passenger;
                        if (!passengerLookup.TryGetValue(passengerRecord.PassengerId.GetValueOrDefault(), out passenger))
                        {
                            passenger = new PassengerInfoDetails
                            {
                                Title = TitleCd.Mnemonic(passengerRecord.TitleCd),
                                FirstName = passengerRecord.FirstName,
                                LastName = passengerRecord.LastName,
                                Type = PassengerTypeCd.Mnemonic(passengerRecord.PassengerTypeCd)
                            };
                            reservation.Passengers.Add(passenger);
                            passengerLookup.Add(passengerRecord.PassengerId.GetValueOrDefault(), passenger);
                        }
                        return reservation;
                    }, "ItineraryId,TripId,SegmentId,PassengerId").Distinct().Single();
                return reservation;
            }
        }

        internal static List<MarginRule> PriceMarginRules()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var activeRuleRecords = GetFlightActivePriceMarginRuleQuery.GetInstance().Execute(conn, null);
                var activeRules = activeRuleRecords.Select(record => new MarginRule
                {
                    RuleId = record.RuleId.GetValueOrDefault(),
                    Name = record.Name,
                    Description = record.Description,
                    BookingDateSpans = record.BookingDateSpans.Deserialize<List<DateSpanRule>>(),
                    BookingDays = record.BookingDays.Deserialize<List<DayOfWeek>>(),
                    BookingDates = record.BookingDates.Deserialize<List<DateTime>>(),
                    FareTypes = record.FareTypes.Deserialize<List<FareType>>(),
                    CabinClasses = record.CabinClasses.Deserialize<List<CabinClass>>(),
                    TripTypes = record.TripTypes.Deserialize<List<TripType>>(),
                    DepartureDateSpans = record.DepartureDateSpans.Deserialize<List<DateSpanRule>>(),
                    DepartureDays = record.DepartureDays.Deserialize<List<DayOfWeek>>(),
                    DepartureDates = record.DepartureDates.Deserialize<List<DateTime>>(),
                    DepartureTimeSpans = record.DepartureTimeSpans.Deserialize<List<TimeSpanRule>>(),
                    ReturnDateSpans = record.ReturnDateSpans.Deserialize<List<DateSpanRule>>(),
                    ReturnDays = record.ReturnDays.Deserialize<List<DayOfWeek>>(),
                    ReturnDates = record.ReturnDates.Deserialize<List<DateTime>>(),
                    ReturnTimeSpans = record.ReturnTimeSpans.Deserialize<List<TimeSpanRule>>(),
                    MaxPassengers = record.MaxPassengers.GetValueOrDefault(),
                    MinPassengers = record.MinPassengers.GetValueOrDefault(),
                    Airlines = record.Airlines.Deserialize<List<string>>(),
                    AirlinesIsExclusion = record.AirlinesIsExclusion.GetValueOrDefault(),
                    AirportPairs = record.AirportPairs.Deserialize<List<AirportPairRule>>(),
                    AirportPairsIsExclusion = record.AirportPairsIsExclusion.GetValueOrDefault(),
                    CityPairs = record.CityPairs.Deserialize<List<AirportPairRule>>(),
                    CityPairsIsExclusion = record.CityPairsIsExclusion.GetValueOrDefault(),
                    CountryPairs = record.CountryPairs.Deserialize<List<AirportPairRule>>(),
                    CountryPairsIsExclusion = record.CountryPairsIsExclusion.GetValueOrDefault(),
                    Coefficient = record.Coefficient.GetValueOrDefault(),
                    Constant = record.ConstraintCount.GetValueOrDefault(),
                    ConstraintCount = record.ConstraintCount.GetValueOrDefault(),
                    Priority = record.Priority.GetValueOrDefault(),
                });
                return activeRules.ToList();
            }
        }
    }
}
