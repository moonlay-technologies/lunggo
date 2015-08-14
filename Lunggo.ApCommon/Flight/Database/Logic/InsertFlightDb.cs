using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Model;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.ApCommon.Flight.Utility;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Flight.Database.Logic
{
    internal class InsertFlightDb
    {
        internal static void Booking(FlightBookingRecord bookingRecord, out string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                //TODO diskon ga seharusnya di sini. konstruk reservasinya sebelum masuk sini.
                var discountRuleIds = VoucherService.GetInstance()
                    .GetFlightDiscountRules(bookingRecord.DiscountCode, bookingRecord.ContactData.Email);
                var discountRule = FlightService.GetInstance().GetMatchingDiscountRule(discountRuleIds);
                var discountNominal = bookingRecord.ItineraryRecords[0].Itinerary.FinalIdrPrice*discountRule.Coefficient +
                                      discountRule.Constant;
                rsvNo = FlightReservationSequence.GetInstance().GetFlightReservationId(EnumReservationType.ReservationType.NonMember);
                var reservationRecord = new FlightReservationTableRecord
                {
                    RsvNo = rsvNo,
                    RsvTime = DateTime.UtcNow,
                    ContactName = bookingRecord.ContactData.Name,
                    ContactEmail = bookingRecord.ContactData.Email,
                    ContactCountryCd = bookingRecord.ContactData.CountryCode,
                    ContactPhone = bookingRecord.ContactData.Phone,
                    LangCd = "xxx",
                    MemberCd = "xxx",
                    CancellationTypeCd = "xxx",
                    PaymentStatusCd = PaymentStatusCd.Mnemonic(PaymentStatus.Pending),
                    OverallTripTypeCd = TripTypeCd.Mnemonic(bookingRecord.OverallTripType),
                    TotalSupplierPrice = bookingRecord.ItineraryRecords[0].Itinerary.FinalIdrPrice,
                    PaymentFeeForCust = 0,
                    PaymentFeeForUs = 0,
                    VoucherCode = bookingRecord.DiscountCode,
                    DiscountId = discountRule.RuleId,
                    DiscountCoefficient = discountRule.Coefficient,
                    DiscountConstant = discountRule.Constant,
                    DiscountNominal = discountNominal,
                    FinalPrice = bookingRecord.ItineraryRecords[0].Itinerary.FinalIdrPrice + discountNominal,
                    GrossProfit = 0,
                    InsertBy = "xxx",
                    InsertDate = DateTime.UtcNow,
                    InsertPgId = "xxx",
                    AdultCount = bookingRecord.Passengers.Count(p => p.Type == PassengerType.Adult),
                    ChildCount = bookingRecord.Passengers.Count(p => p.Type == PassengerType.Child),
                    InfantCount = bookingRecord.Passengers.Count(p => p.Type == PassengerType.Infant)
                };

                FlightReservationTableRepo.GetInstance().Insert(conn, reservationRecord);

                foreach (var record in bookingRecord.ItineraryRecords)
                {
                    var itineraryId = FlightItineraryIdSequence.GetInstance().GetNext();
                    var itineraryRecord = new FlightItineraryTableRecord
                    {
                        ItineraryId = itineraryId,
                        RsvNo = rsvNo,
                        BookingId = record.BookResult.BookingId,
                        BookingStatusCd = BookingStatusCd.Mnemonic(BookingStatus.Booked),
                        FareTypeCd = FareTypeCd.Mnemonic(FlightIdUtil.GetFareType(record.BookResult.BookingId)),
                        SupplierCd = FlightSupplierCd.Mnemonic(FlightIdUtil.GetSupplier(record.BookResult.BookingId)),
                        SupplierPrice = record.Itinerary.SupplierPrice,
                        SupplierCurrencyCd = record.Itinerary.SupplierCurrency,
                        SupplierExchangeRate = record.Itinerary.SupplierRate,
                        OriginalIdrPrice = record.Itinerary.OriginalIdrPrice,
                        MarginId = record.Itinerary.MarginId,
                        MarginCoefficient = record.Itinerary.MarginCoefficient,
                        MarginConstant = record.Itinerary.MarginConstant,
                        MarginNominal = record.Itinerary.MarginNominal,
                        FinalIdrPrice = record.Itinerary.FinalIdrPrice,
                        LocalPrice = record.Itinerary.LocalPrice,
                        LocalCurrencyCd = record.Itinerary.LocalCurrency,
                        LocalExchangeRate = record.Itinerary.LocalRate,
                        TripTypeCd = TripTypeCd.Mnemonic(record.Itinerary.TripType),
                        InsertBy = "xxx",
                        InsertDate = DateTime.UtcNow,
                        InsertPgId = "xxx"
                    };
                    FlightItineraryTableRepo.GetInstance().Insert(conn, itineraryRecord);

                    foreach (var trip in record.Itinerary.FlightTrips)
                    {
                        var tripId = FlightTripIdSequence.GetInstance().GetNext();
                        var tripRecord = new FlightTripTableRecord
                        {
                            TripId = tripId,
                            ItineraryId = itineraryId,
                            OriginAirportCd = trip.OriginAirport,
                            DestinationAirportCd = trip.DestinationAirport,
                            DepartureDate = trip.DepartureDate,
                            InsertBy = "xxx",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "xxx"
                        };
                        FlightTripTableRepo.GetInstance().Insert(conn, tripRecord);

                        var segments = trip.FlightSegments;
                        foreach (var segment in segments)
                        {
                            var segmentId = FlightSegmentIdSequence.GetInstance().GetNext();
                            var segmentRecord = new FlightSegmentTableRecord
                            {
                                SegmentId = segmentId,
                                TripId = tripId,
                                DepartureAirportCd = segment.DepartureAirport,
                                DepartureTime = segment.DepartureTime,
                                ArrivalAirportCd = segment.ArrivalAirport,
                                ArrivalTime = segment.ArrivalTime,
                                AirlineCd = segment.AirlineCode,
                                OperatingAirlineCd = segment.OperatingAirlineCode,
                                FlightNumber = segment.FlightNumber,
                                AircraftCd = segment.AircraftCode,
                                Duration = segment.Duration,
                                StopQuantity = segment.StopQuantity,
                                InsertBy = "xxx",
                                InsertDate = DateTime.UtcNow,
                                InsertPgId = "xxx"
                            };
                            FlightSegmentTableRepo.GetInstance().Insert(conn, segmentRecord);

                            if (segment.StopQuantity > 0)
                            {
                                foreach (var stop in segment.FlightStops)
                                {
                                    var stopId = FlightStopIdSequence.GetInstance().GetNext();
                                    var stopRecord = new FlightStopTableRecord
                                    {
                                        StopId = stopId,
                                        SegmentId = segmentId,
                                        ArrivalTime = stop.ArrivalTime,
                                        DepartureTime = stop.DepartureTime,
                                        Duration = stop.Duration,
                                        AirportCd = stop.Airport,
                                        InsertBy = "xxx",
                                        InsertDate = DateTime.UtcNow,
                                        InsertPgId = "xxx"
                                    };
                                    FlightStopTableRepo.GetInstance().Insert(conn, stopRecord);
                                }
                            }
                        }
                    }

                    foreach (var passenger in bookingRecord.Passengers)
                    {

                        var passengerRecord = new FlightPassengerTableRecord
                        {
                            PassengerId = FlightPassengerIdSequence.GetInstance().GetNext(),
                            RsvNo = rsvNo,
                            PassengerTypeCd = PassengerTypeCd.Mnemonic(passenger.Type),
                            GenderCd = GenderCd.Mnemonic(passenger.Gender),
                            TitleCd = TitleCd.Mnemonic(passenger.Title),
                            FirstName = passenger.FirstName,
                            LastName = passenger.LastName,
                            BirthDate = passenger.DateOfBirth,
                            CountryCd = passenger.PassportCountry,
                            IdNumber = passenger.PassportNumber,
                            PassportExpiryDate = passenger.PassportExpiryDate,
                            InsertBy = "xxx",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "xxx"
                        }; ;
                        FlightPassengerTableRepo.GetInstance().Insert(conn, passengerRecord);
                    }
                }
            }
        }

        internal static void Details(GetTripDetailsResult details)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var itineraryId = GetFlightItineraryIdQuery.GetInstance().Execute(conn, new {details.BookingId}).Single();

                foreach (var trip in details.FlightItineraries.FlightTrips)
                {
                    var tripId = FlightTripIdSequence.GetInstance().GetNext();
                    var tripRecord = new FlightTripTableRecord
                    {
                        TripId = tripId,
                        ItineraryId = itineraryId,
                        OriginAirportCd = trip.OriginAirport,
                        DestinationAirportCd = trip.DestinationAirport,
                        DepartureDate = trip.DepartureDate,
                        InsertBy = "xxx",
                        InsertDate = DateTime.UtcNow,
                        InsertPgId = "xxx"
                    };
                    FlightTripTableRepo.GetInstance().Insert(conn, tripRecord);

                    var segments = trip.FlightSegments;
                    foreach (var segment in segments)
                    {
                        var segmentId = FlightSegmentIdSequence.GetInstance().GetNext();
                        var segmentRecord = new FlightSegmentTableRecord
                        {
                            SegmentId = segmentId,
                            TripId = tripId,
                            Pnr = segment.Pnr,
                            DepartureAirportCd = segment.DepartureAirport,
                            DepartureTime = segment.DepartureTime,
                            ArrivalAirportCd = segment.ArrivalAirport,
                            ArrivalTime = segment.ArrivalTime,
                            AirlineCd = segment.AirlineCode,
                            OperatingAirlineCd = segment.OperatingAirlineCode,
                            FlightNumber = segment.FlightNumber,
                            AircraftCd = segment.AircraftCode,
                            Duration = segment.Duration,
                            StopQuantity = segment.StopQuantity,
                            InsertBy = "xxx",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "xxx"
                        };
                        FlightSegmentTableRepo.GetInstance().Insert(conn, segmentRecord);
                    }
                }
            }
        }

        internal static void PriceMarginRules(List<MarginRule> rules, List<MarginRule> deletedRules)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var allRules = FlightPriceMarginRuleTableRepo.GetInstance().FindAll(conn).ToList();
                var activeRules = allRules.Where(rule => rule.IsActive.GetValueOrDefault()).ToList();
                var inactiveRules = allRules.Where(rule => !rule.IsActive.GetValueOrDefault()).ToList();
                foreach (var rule in rules)
                {
                    if (activeRules.Any(activeRule => activeRule.RuleId == rule.RuleId))
                    {
                        FlightPriceMarginRuleTableRepo.GetInstance().Update(conn, new FlightPriceMarginRuleTableRecord
                        {
                            RuleId = rule.RuleId,
                            Priority = rule.Priority
                        });
                    }
                    else
                    {
                        var ruleRecord = new FlightPriceMarginRuleTableRecord
                        {
                            RuleId = rule.RuleId,
                            Name = rule.Name,
                            Description = rule.Description,
                            BookingDateSpans = rule.BookingDateSpans.Serialize(),
                            BookingDays = rule.BookingDays.Serialize(),
                            BookingDates = rule.BookingDates.Serialize(),
                            FareTypes = rule.FareTypes.Serialize(),
                            CabinClasses = rule.CabinClasses.Serialize(),
                            TripTypes = rule.TripTypes.Serialize(),
                            DepartureDateSpans = rule.DepartureDateSpans.Serialize(),
                            DepartureDays = rule.DepartureDays.Serialize(),
                            DepartureDates = rule.DepartureDates.Serialize(),
                            DepartureTimeSpans = rule.DepartureTimeSpans.Serialize(),
                            ReturnDateSpans = rule.ReturnDateSpans.Serialize(),
                            ReturnDays = rule.ReturnDays.Serialize(),
                            ReturnDates = rule.ReturnDates.Serialize(),
                            ReturnTimeSpans = rule.ReturnTimeSpans.Serialize(),
                            MaxPassengers = rule.MaxPassengers,
                            MinPassengers = rule.MinPassengers,
                            Airlines = rule.Airlines.Serialize(),
                            AirlinesIsExclusion = rule.AirlinesIsExclusion,
                            AirportPairs = rule.AirportPairs.Serialize(),
                            AirportPairsIsExclusion = rule.AirportPairsIsExclusion,
                            CityPairs = rule.CityPairs.Serialize(),
                            CityPairsIsExclusion = rule.CityPairsIsExclusion,
                            CountryPairs = rule.CountryPairs.Serialize(),
                            CountryPairsIsExclusion = rule.CountryPairsIsExclusion,
                            Coefficient = rule.Coefficient,
                            Constant = rule.ConstraintCount,
                            ConstraintCount = rule.ConstraintCount,
                            Priority = rule.Priority,
                            IsActive = true,
                            InsertBy = "xxx",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "xxx"
                        };
                        FlightPriceMarginRuleTableRepo.GetInstance().Insert(conn, ruleRecord);
                    }
                }
                foreach (var deletedRule in deletedRules)
                {
                    if (inactiveRules.Any(inactiveRule => inactiveRule.RuleId == deletedRule.RuleId))
                    {
                        FlightPriceMarginRuleTableRepo.GetInstance().Update(conn, new FlightPriceMarginRuleTableRecord
                        {
                            RuleId = deletedRule.RuleId,
                            IsActive = false
                        });
                    }
                }
            }
        }
    }
}
