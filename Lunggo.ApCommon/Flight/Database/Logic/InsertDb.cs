using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Query;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Context;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        internal class InsertDb
        {
            internal static void Reservation(FlightReservation reservation)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var reservationRecord = new FlightReservationTableRecord
                    {
                        RsvNo = reservation.RsvNo,
                        RsvTime = reservation.RsvTime.ToUniversalTime(),
                        ContactName = reservation.Contact.Name,
                        ContactEmail = reservation.Contact.Email,
                        ContactCountryCd = reservation.Contact.CountryCode,
                        ContactPhone = reservation.Contact.Phone,
                        LangCd = OnlineContext.GetActiveLanguageCode() ?? "",
                        MemberCd = "xxx",
                        CancellationTypeCd = "xxx",
                        AdultCount = reservation.Passengers.Count(p => p.Type == PassengerType.Adult),
                        ChildCount = reservation.Passengers.Count(p => p.Type == PassengerType.Child),
                        InfantCount = reservation.Passengers.Count(p => p.Type == PassengerType.Infant),
                        OverallTripTypeCd = TripTypeCd.Mnemonic(reservation.OverallTripType),
                        TotalSupplierPrice = reservation.Itineraries.Sum(itin => itin.FinalIdrPrice),
                        PaymentFeeForCust = 0,
                        PaymentFeeForUs = 0,
                        GrossProfit = 0,
                        InsertBy = "xxx",
                        InsertDate = DateTime.UtcNow,
                        InsertPgId = "xxx",
                        TransferCode = reservation.TransferCode
                    };

                    FlightReservationTableRepo.GetInstance().Insert(conn, reservationRecord);

                    foreach (var itin in reservation.Itineraries)
                    {
                        var itineraryId = FlightItineraryIdSequence.GetInstance().GetNext();
                        var itineraryRecord = new FlightItineraryTableRecord
                        {
                            ItineraryId = itineraryId,
                            RsvNo = reservation.RsvNo,
                            BookingId = itin.BookingId,
                            BookingStatusCd = BookingStatusCd.Mnemonic(BookingStatus.Booked),
                            TicketTimeLimit = itin.TicketTimeLimit,
                            FareTypeCd = FareTypeCd.Mnemonic(IdUtil.GetFareType(itin.BookingId)),
                            CanHold = itin.CanHold,
                            SupplierCd = SupplierCd.Mnemonic(IdUtil.GetSupplier(itin.BookingId)),
                            SupplierPrice = itin.SupplierPrice,
                            SupplierCurrencyCd = itin.SupplierCurrency,
                            SupplierExchangeRate = itin.SupplierRate,
                            OriginalIdrPrice = itin.OriginalIdrPrice,
                            MarginId = itin.MarginId,
                            MarginCoefficient = itin.MarginCoefficient,
                            MarginConstant = itin.MarginConstant,
                            MarginNominal = itin.MarginNominal,
                            MarginIsFlat = itin.MarginIsFlat,
                            FinalIdrPrice = itin.FinalIdrPrice,
                            LocalPrice = itin.LocalPrice,
                            LocalCurrencyCd = itin.LocalCurrency,
                            LocalExchangeRate = itin.LocalRate,
                            TripTypeCd = TripTypeCd.Mnemonic(itin.TripType),
                            InsertBy = "xxx",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "xxx",
                        };
                        FlightItineraryTableRepo.GetInstance().Insert(conn, itineraryRecord);

                        foreach (var trip in itin.Trips)
                        {
                            var tripId = FlightTripIdSequence.GetInstance().GetNext();
                            var tripRecord = new FlightTripTableRecord
                            {
                                TripId = tripId,
                                ItineraryId = itineraryId,
                                OriginAirportCd = trip.OriginAirport,
                                DestinationAirportCd = trip.DestinationAirport,
                                DepartureDate = trip.DepartureDate.ToUniversalTime(),
                                InsertBy = "xxx",
                                InsertDate = DateTime.UtcNow,
                                InsertPgId = "xxx"
                            };
                            FlightTripTableRepo.GetInstance().Insert(conn, tripRecord);

                            var segments = trip.Segments;
                            foreach (var segment in segments)
                            {
                                var segmentId = FlightSegmentIdSequence.GetInstance().GetNext();
                                var segmentRecord = new FlightSegmentTableRecord
                                {
                                    SegmentId = segmentId,
                                    TripId = tripId,
                                    DepartureAirportCd = segment.DepartureAirport,
                                    DepartureTime = segment.DepartureTime.ToUniversalTime(),
                                    ArrivalAirportCd = segment.ArrivalAirport,
                                    ArrivalTime = segment.ArrivalTime.ToUniversalTime(),
                                    AirlineCd = segment.AirlineCode,
                                    OperatingAirlineCd = segment.OperatingAirlineCode,
                                    FlightNumber = segment.FlightNumber,
                                    AircraftCd = segment.AircraftCode,
                                    Duration = segment.Duration,
                                    CabinClassCd = CabinClassCd.Mnemonic(segment.CabinClass),
                                    StopQuantity = segment.StopQuantity,
                                    InsertBy = "xxx",
                                    InsertDate = DateTime.UtcNow,
                                    InsertPgId = "xxx"
                                };
                                FlightSegmentTableRepo.GetInstance().Insert(conn, segmentRecord);

                                if (segment.StopQuantity > 0)
                                {
                                    foreach (var stop in segment.Stops)
                                    {
                                        var stopId = FlightStopIdSequence.GetInstance().GetNext();
                                        var stopRecord = new FlightStopTableRecord
                                        {
                                            StopId = stopId,
                                            SegmentId = segmentId,
                                            ArrivalTime = stop.ArrivalTime.ToUniversalTime(),
                                            DepartureTime = stop.DepartureTime.ToUniversalTime(),
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
                    }
                    foreach (var passenger in reservation.Passengers)
                    {

                        var passengerRecord = new FlightPassengerTableRecord
                        {
                            PassengerId = FlightPassengerIdSequence.GetInstance().GetNext(),
                            RsvNo = reservation.RsvNo,
                            PassengerTypeCd = PassengerTypeCd.Mnemonic(passenger.Type),
                            GenderCd = GenderCd.Mnemonic(passenger.Gender),
                            TitleCd = TitleCd.Mnemonic(passenger.Title),
                            FirstName = passenger.FirstName,
                            LastName = passenger.LastName,
                            BirthDate = passenger.DateOfBirth.HasValue ? passenger.DateOfBirth.Value.ToUniversalTime() : (DateTime?) null,
                            CountryCd = passenger.PassportCountry,
                            IdNumber = passenger.PassportNumber,
                            PassportExpiryDate = passenger.PassportExpiryDate.HasValue ? passenger.PassportExpiryDate.Value.ToUniversalTime() : (DateTime?) null,
                            InsertBy = "xxx",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "xxx"
                        };
                        FlightPassengerTableRepo.GetInstance().Insert(conn, passengerRecord);
                    }

                }
            }

            internal static void Details(GetTripDetailsResult details)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var itineraryId = GetItineraryIdQuery.GetInstance().Execute(conn, new {details.BookingId}).Single();

                    foreach (var trip in details.Itinerary.Trips)
                    {
                        var tripId = FlightTripIdSequence.GetInstance().GetNext();
                        var tripRecord = new FlightTripTableRecord
                        {
                            TripId = tripId,
                            ItineraryId = itineraryId,
                            OriginAirportCd = trip.OriginAirport,
                            DestinationAirportCd = trip.DestinationAirport,
                            DepartureDate = trip.DepartureDate.ToUniversalTime(),
                            InsertBy = "xxx",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "xxx"
                        };
                        FlightTripTableRepo.GetInstance().Insert(conn, tripRecord);

                        var segments = trip.Segments;
                        foreach (var segment in segments)
                        {
                            var segmentId = FlightSegmentIdSequence.GetInstance().GetNext();
                            var segmentRecord = new FlightSegmentTableRecord
                            {
                                SegmentId = segmentId,
                                TripId = tripId,
                                Pnr = segment.Pnr,
                                DepartureAirportCd = segment.DepartureAirport,
                                DepartureTime = segment.DepartureTime.ToUniversalTime(),
                                ArrivalAirportCd = segment.ArrivalAirport,
                                ArrivalTime = segment.ArrivalTime.ToUniversalTime(),
                                AirlineCd = segment.AirlineCode,
                                OperatingAirlineCd = segment.OperatingAirlineCode,
                                FlightNumber = segment.FlightNumber,
                                AircraftCd = segment.AircraftCode,
                                Duration = segment.Duration,
                                CabinClassCd = CabinClassCd.Mnemonic(segment.CabinClass),
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

            internal static void SavedPassengers(string contactEmail, List<FlightPassenger> passengers)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var savedPassengers = GetSavedPassengersByContactEmailQuery.GetInstance()
                        .Execute(conn, new {ContactEmail = contactEmail}).ToList();
                    foreach (var passenger in passengers)
                    {
                        var passengerRecord = new FlightSavedPassengerTableRecord
                        {
                            ContactEmail = contactEmail,
                            PassengerTypeCd = PassengerTypeCd.Mnemonic(passenger.Type),
                            TitleCd = TitleCd.Mnemonic(passenger.Title),
                            FirstName = passenger.FirstName,
                            LastName = passenger.LastName,
                            BirthDate = passenger.DateOfBirth,
                            GenderCd = GenderCd.Mnemonic(passenger.Title == Title.Mister ? Gender.Male : Gender.Female),
                            IdNumber = passenger.PassportNumber,
                            PassportExpiryDate = passenger.PassportExpiryDate,
                            CountryCd = passenger.PassportCountry
                        };
                        if (savedPassengers.Any(
                            saved => String.Equals(saved.ContactEmail, contactEmail, StringComparison.CurrentCultureIgnoreCase) &&
                                String.Equals(saved.FirstName, passenger.FirstName, StringComparison.CurrentCultureIgnoreCase) && 
                                String.Equals(saved.LastName, passenger.LastName, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            FlightSavedPassengerTableRepo.GetInstance().Update(conn, passengerRecord);
                        }
                        else
                        {
                            FlightSavedPassengerTableRepo.GetInstance().Insert(conn, passengerRecord);
                        }
                    }
                }
            }

            internal static void PriceMarginRules(List<MarginRule> rules, List<MarginRule> deletedRules)
            {
                using (var conn = DbService.GetInstance().GetOpenConnection())
                {
                    var activeRules = GetActivePriceMarginRuleQuery.GetInstance().Execute(conn, null).ToList();
                    foreach (var deletedRule in deletedRules)
                    {
                        if (activeRules.Any(activeRule => activeRule.RuleId == deletedRule.RuleId))
                        {
                            FlightPriceMarginRuleTableRepo.GetInstance()
                                .Update(conn, new FlightPriceMarginRuleTableRecord
                                {
                                    RuleId = deletedRule.RuleId,
                                    Priority = deletedRule.Priority,
                                    IsActive = false
                                });
                        }
                    }
                    foreach (var rule in rules)
                    {
                        if (activeRules.Any(activeRule => activeRule.RuleId == rule.RuleId))
                        {
                            FlightPriceMarginRuleTableRepo.GetInstance()
                                .Update(conn, new FlightPriceMarginRuleTableRecord
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
                                IsFlat = rule.IsFlat,
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
                }
            }
        }
    }
}