using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        internal class GetDb
        {
            internal static List<FlightReservation> OverviewReservationsByContactEmail(string contactEmail)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var rsvNos =
                        GetRsvNosByContactEmailQuery.GetInstance()
                            .Execute(conn, new {ContactEmail = contactEmail})
                            .ToList();
                    if (!rsvNos.Any())
                        return null;
                    else
                    {
                        return rsvNos.Select(OverviewReservation).ToList();
                    }
                }
            }

            internal static IEnumerable<FlightReservation> SearchReservations(FlightReservationSearch search)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var rsvNos = SearchReservationQuery.GetInstance().Execute(conn, search, search);
                    var reservations = rsvNos.Select(Reservation);
                    return reservations;
                }
            }

            internal static FlightReservation OverviewReservation(string rsvNo)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    FlightReservation reservation = null;
                    var itineraryLookup = new Dictionary<long, FlightItinerary>();
                    var tripLookup = new Dictionary<long, FlightTrip>();
                    var segmentLookup = new Dictionary<long, FlightSegment>();
                    var passengerLookup = new Dictionary<long, FlightPassenger>();
                    var dict = DictionaryService.GetInstance();
                    reservation = GetReservationQuery.GetInstance().ExecuteMultiMap(conn, new {RsvNo = rsvNo},
                        (reservationRecord, itineraryRecord, tripRecord, segmentRecord, passengerRecord) =>
                        {
                            if (reservation == null)
                            {
                                reservation = new FlightReservation
                                {
                                    RsvNo = rsvNo,
                                    RsvTime = reservationRecord.RsvTime.GetValueOrDefault(),
                                    InvoiceNo = reservationRecord.InvoiceNo,
                                    Contact = new ContactData
                                    {
                                        Name = reservationRecord.ContactName,
                                        Email = reservationRecord.ContactEmail,
                                        CountryCode = reservationRecord.ContactCountryCd,
                                        Phone = reservationRecord.ContactPhone
                                    },
                                    Payment = new PaymentInfo
                                    {
                                        Status = PaymentStatusCd.Mnemonic(reservationRecord.PaymentStatusCd),
                                        FinalPrice = reservationRecord.FinalPrice.GetValueOrDefault(),
                                        Currency = reservationRecord.CurrencyCd,
                                        Time = reservationRecord.PaymentTime,
                                        TimeLimit = reservationRecord.PaymentTimeLimit,
                                        Medium = PaymentMediumCd.Mnemonic(reservationRecord.PaymentMediumCd),
                                        Method = PaymentMethodCd.Mnemonic(reservationRecord.PaymentMethodCd),
                                        Url = reservationRecord.PaymentUrl
                                    },
                                    Discount = new DiscountData
                                    {
                                        Code = reservationRecord.VoucherCode,
                                        Id = reservationRecord.DiscountId.GetValueOrDefault(),
                                        Percentage = reservationRecord.DiscountPercentage.GetValueOrDefault(),
                                        Constant = reservationRecord.DiscountConstant.GetValueOrDefault(),
                                        Nominal = reservationRecord.DiscountNominal.GetValueOrDefault()
                                    },
                                    TripType = TripTypeCd.Mnemonic(reservationRecord.OverallTripTypeCd),
                                    Itineraries = new List<FlightItinerary>(),
                                    Passengers = new List<FlightPassenger>()
                                };
                            }
                            FlightItinerary itinerary;
                            if (
                                !itineraryLookup.TryGetValue(itineraryRecord.ItineraryId.GetValueOrDefault(),
                                    out itinerary))
                            {
                                itinerary = new FlightItinerary
                                {
                                    Trips = new List<FlightTrip>()
                                };
                                itineraryLookup.Add(itineraryRecord.ItineraryId.GetValueOrDefault(), itinerary);
                                reservation.Itineraries.Add(itinerary);
                            }
                            FlightTrip trip;
                            if (!tripLookup.TryGetValue(tripRecord.TripId.GetValueOrDefault(), out trip))
                            {
                                trip = new FlightTrip
                                {
                                    OriginAirport = tripRecord.OriginAirportCd,
                                    OriginAirportName = dict.GetAirportName(tripRecord.OriginAirportCd),
                                    OriginCity = dict.GetAirportCity(tripRecord.OriginAirportCd),
                                    DestinationAirport = tripRecord.DestinationAirportCd,
                                    DestinationAirportName = dict.GetAirportName(tripRecord.DestinationAirportCd),
                                    DestinationCity = dict.GetAirportCity(tripRecord.DestinationAirportCd),
                                    DepartureDate = tripRecord.DepartureDate.GetValueOrDefault(),
                                    Segments = new List<FlightSegment>()
                                };
                                tripLookup.Add(tripRecord.TripId.GetValueOrDefault(), trip);
                                itinerary.Trips.Add(trip);
                            }
                            FlightSegment segment;
                            if (!segmentLookup.TryGetValue(segmentRecord.SegmentId.GetValueOrDefault(), out segment))
                            {
                                segment = new FlightSegment
                                {
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
                                    CabinClass = CabinClassCd.Mnemonic(segmentRecord.CabinClassCd),
                                    Baggage = segmentRecord.Baggage
                                };
                                segmentLookup.Add(segmentRecord.SegmentId.GetValueOrDefault(), segment);
                                trip.Segments.Add(segment);
                            }
                            FlightPassenger passenger;
                            if (
                                !passengerLookup.TryGetValue(passengerRecord.PassengerId.GetValueOrDefault(),
                                    out passenger))
                            {
                                passenger = new FlightPassenger
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
                    var itineraryLookup = new Dictionary<long, FlightItinerary>();
                    var tripLookup = new Dictionary<long, FlightTrip>();
                    var segmentLookup = new Dictionary<long, FlightSegment>();
                    var passengerLookup = new Dictionary<long, FlightPassenger>();
                    var dict = DictionaryService.GetInstance();
                    reservation = GetReservationQuery.GetInstance().ExecuteMultiMap(conn, new {RsvNo = rsvNo},
                        (reservationRecord, itineraryRecord, tripRecord, segmentRecord, passengerRecord) =>
                        {
                            if (reservationRecord == null)
                                return reservation;
                            if (reservation == null)
                            {
                                RefundInfo refundInfo = null;
                                if (reservationRecord.RefundTime != null)
                                {
                                    refundInfo = new RefundInfo
                                    {
                                        Amount = reservationRecord.RefundAmount.GetValueOrDefault(),
                                        Time = reservationRecord.RefundTime.GetValueOrDefault(),
                                        TargetBank = reservationRecord.RefundTargetBank,
                                        TargetAccount = reservationRecord.RefundTargetAccount
                                    };
                                }
                                reservation = new FlightReservation
                                {
                                    RsvNo = rsvNo,
                                    RsvTime = reservationRecord.RsvTime.GetValueOrDefault(),
                                    InvoiceNo = reservationRecord.InvoiceNo,
                                    Contact = new ContactData
                                    {
                                        Name = reservationRecord.ContactName,
                                        Email = reservationRecord.ContactEmail,
                                        CountryCode = reservationRecord.ContactCountryCd,
                                        Phone = reservationRecord.ContactPhone
                                    },
                                    Payment = new PaymentInfo
                                    {
                                        Id = reservationRecord.PaymentId,
                                        Medium = PaymentMediumCd.Mnemonic(reservationRecord.PaymentMediumCd),
                                        Method = PaymentMethodCd.Mnemonic(reservationRecord.PaymentMethodCd),
                                        TimeLimit = reservationRecord.PaymentTimeLimit.GetValueOrDefault(),
                                        Time = reservationRecord.PaymentTime,
                                        Status = PaymentStatusCd.Mnemonic(reservationRecord.PaymentStatusCd),
                                        TargetAccount = reservationRecord.PaymentTargetAccount,
                                        Url = reservationRecord.PaymentUrl,
                                        FinalPrice = reservationRecord.FinalPrice.GetValueOrDefault(),
                                        PaidAmount = reservationRecord.PaidAmount.GetValueOrDefault(),
                                        Currency = reservationRecord.CurrencyCd,
                                        Refund = refundInfo
                                    },
                                    Discount = new DiscountData
                                    {
                                        Code = reservationRecord.VoucherCode,
                                        Id = reservationRecord.DiscountId.GetValueOrDefault(),
                                        Percentage = reservationRecord.DiscountPercentage.GetValueOrDefault(),
                                        Constant = reservationRecord.DiscountConstant.GetValueOrDefault(),
                                        Nominal = reservationRecord.DiscountNominal.GetValueOrDefault()
                                    },
                                    TripType = TripTypeCd.Mnemonic(reservationRecord.OverallTripTypeCd),
                                    Itineraries = new List<FlightItinerary>(),
                                    Passengers = new List<FlightPassenger>()
                                };
                            }
                            FlightItinerary itinerary;
                            if (
                                !itineraryLookup.TryGetValue(itineraryRecord.ItineraryId.GetValueOrDefault(),
                                    out itinerary))
                            {
                                itinerary = new FlightItinerary
                                {
                                    BookingId = itineraryRecord.BookingId,
                                    BookingStatus = BookingStatusCd.Mnemonic(itineraryRecord.BookingStatusCd),
                                    TripType = TripTypeCd.Mnemonic(itineraryRecord.TripTypeCd),
                                    Supplier = SupplierCd.Mnemonic(itineraryRecord.SupplierCd),
                                    TicketTimeLimit = itineraryRecord.TicketTimeLimit,
                                    CanHold = itineraryRecord.CanHold.GetValueOrDefault(),
                                    MarginId = itineraryRecord.MarginId.GetValueOrDefault(),
                                    MarginCoefficient = itineraryRecord.MarginCoefficient.GetValueOrDefault(),
                                    MarginConstant = itineraryRecord.MarginConstant.GetValueOrDefault(),
                                    MarginNominal = itineraryRecord.MarginNominal.GetValueOrDefault(),
                                    Trips = new List<FlightTrip>()
                                };
                                itineraryLookup.Add(itineraryRecord.ItineraryId.GetValueOrDefault(), itinerary);
                                reservation.Itineraries.Add(itinerary);
                            }
                            FlightTrip trip;
                            if (!tripLookup.TryGetValue(tripRecord.TripId.GetValueOrDefault(), out trip))
                            {
                                trip = new FlightTrip
                                {
                                    OriginAirport = tripRecord.OriginAirportCd,
                                    OriginAirportName = dict.GetAirportName(tripRecord.OriginAirportCd),
                                    OriginCity = dict.GetAirportCity(tripRecord.OriginAirportCd),
                                    DestinationAirport = tripRecord.DestinationAirportCd,
                                    DestinationAirportName = dict.GetAirportName(tripRecord.DestinationAirportCd),
                                    DestinationCity = dict.GetAirportCity(tripRecord.DestinationAirportCd),
                                    DepartureDate = tripRecord.DepartureDate.GetValueOrDefault(),
                                    Segments = new List<FlightSegment>()
                                };
                                tripLookup.Add(tripRecord.TripId.GetValueOrDefault(), trip);
                                itinerary.Trips.Add(trip);
                            }
                            FlightSegment segment;
                            if (!segmentLookup.TryGetValue(segmentRecord.SegmentId.GetValueOrDefault(), out segment))
                            {
                                segment = new FlightSegment
                                {
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
                                    Duration = segmentRecord.Duration.GetValueOrDefault(),
                                    CabinClass = CabinClassCd.Mnemonic(segmentRecord.CabinClassCd),
                                    Baggage = segmentRecord.Baggage,
                                    Pnr = segmentRecord.Pnr
                                };
                                segmentLookup.Add(segmentRecord.SegmentId.GetValueOrDefault(), segment);
                                trip.Segments.Add(segment);
                            }
                            FlightPassenger passenger;
                            if (
                                !passengerLookup.TryGetValue(passengerRecord.PassengerId.GetValueOrDefault(),
                                    out passenger))
                            {
                                passenger = new FlightPassenger
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

            internal static IEnumerable<FlightReservation> UnpaidReservations()
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var rsvRecords = GetUnpaidReservationQuery.GetInstance().Execute(conn, null);
                    var reservations = rsvRecords.Select(record => new FlightReservation
                    {
                        RsvNo = record.RsvNo,
                        Payment = new PaymentInfo
                        {
                            FinalPrice = record.FinalPrice.GetValueOrDefault(),
                            TimeLimit = record.PaymentTimeLimit
                        }
                    });
                    return reservations;
                }
            }

            internal static List<MarginRule> ActivePriceMarginRules()
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var activeRuleRecords = GetActivePriceMarginRuleQuery.GetInstance().Execute(conn, null);
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

            internal static List<string> RsvNoByBookingId(List<string> bookingIds)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    return GetRsvNoByBookingIdQuery.GetInstance().Execute(conn, new {BookingIds = bookingIds}).Distinct().ToList();
                }
            }

            internal static PaymentStatus PaymentStatus(string rsvNo)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var statusCd = GetPaymentStatusQuery.GetInstance().Execute(conn, new {RsvNo = rsvNo}).Single();
                    var status = PaymentStatusCd.Mnemonic(statusCd);
                    return status;
                }
            }

            internal static List<FlightPassenger> SavedPassengers(string contactEmail)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var passengerRecords =
                        GetSavedPassengersByContactEmailQuery.GetInstance().Execute(conn, new {ContactEmail = contactEmail}).ToList();
                    return passengerRecords.Select(record => new FlightPassenger
                    {
                        Type = PassengerTypeCd.Mnemonic(record.PassengerTypeCd),
                        Title = TitleCd.Mnemonic(record.TitleCd),
                        FirstName = record.FirstName,
                        LastName = record.LastName,
                        Gender = GenderCd.Mnemonic(record.GenderCd),
                        DateOfBirth = record.BirthDate,
                        PassportNumber = record.IdNumber,
                        PassportExpiryDate = record.PassportExpiryDate,
                        PassportCountry = record.CountryCd
                    }).ToList();
                }
            }
        }
    }
}