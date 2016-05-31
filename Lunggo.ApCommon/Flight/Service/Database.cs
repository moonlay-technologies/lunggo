using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.ApCommon.Identity.User;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.ApCommon.ProductBase.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Redis;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using StackExchange.Redis;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        #region Get
        private List<FlightReservation> GetOverviewReservationsFromDb(string contactEmail)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var rsvNos =
                    GetRsvNosByContactEmailQuery.GetInstance()
                        .Execute(conn, new { Email = contactEmail })
                        .ToList();
                if (!rsvNos.Any())
                    return null;
                else
                {
                    return rsvNos.Select(GetOverviewReservationFromDb).ToList();
                }
            }
        }

        private IEnumerable<FlightReservation> GetSearchReservationsFromDb(FlightReservationSearch search)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var rsvNos = SearchReservationQuery.GetInstance().Execute(conn, search, search);
                var reservations = rsvNos.Select(GetReservationFromDb);
                return reservations;
            }
        }

        private FlightReservation GetOverviewReservationFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                FlightReservation reservation = null;
                var itineraryLookup = new Dictionary<long, FlightItinerary>();
                var tripLookup = new Dictionary<long, FlightTrip>();
                var segmentLookup = new Dictionary<long, FlightSegment>();
                var passengerLookup = new Dictionary<long, FlightPassenger>();
                reservation = GetReservationQuery.GetInstance().ExecuteMultiMap(conn, new { RsvNo = rsvNo },
                    (reservationRecord, itineraryRecord, tripRecord, segmentRecord, passengerRecord, stopRecord) =>
                    {
                        if (reservation == null)
                        {
                            reservation = new FlightReservation
                            {
                                RsvNo = rsvNo,
                                RsvTime = reservationRecord.RsvTime.GetValueOrDefault(),
                                Contact = Contact.GetFromDb(rsvNo),
                                Payment = PaymentDetails.GetFromDb(rsvNo),
                                OverallTripType = TripTypeCd.Mnemonic(reservationRecord.OverallTripTypeCd),
                                Itineraries = new List<FlightItinerary>(),
                                Passengers = new List<FlightPassenger>()
                            };
                        }
                        FlightItinerary itinerary;
                        if (
                            !itineraryLookup.TryGetValue(itineraryRecord.Id.GetValueOrDefault(),
                                out itinerary))
                        {
                            itinerary = new FlightItinerary
                            {
                                Price = Price.GetFromDb(itineraryRecord.Id.GetValueOrDefault()),
                                Trips = new List<FlightTrip>()
                            };
                            itineraryLookup.Add(itineraryRecord.Id.GetValueOrDefault(), itinerary);
                            reservation.Itineraries.Add(itinerary);
                        }
                        FlightTrip trip;
                        if (!tripLookup.TryGetValue(tripRecord.Id.GetValueOrDefault(), out trip))
                        {
                            trip = new FlightTrip
                            {
                                OriginAirport = tripRecord.OriginAirportCd,
                                OriginAirportName = GetAirportName(tripRecord.OriginAirportCd),
                                OriginCity = GetAirportCity(tripRecord.OriginAirportCd),
                                DestinationAirport = tripRecord.DestinationAirportCd,
                                DestinationAirportName = GetAirportName(tripRecord.DestinationAirportCd),
                                DestinationCity = GetAirportCity(tripRecord.DestinationAirportCd),
                                DepartureDate = tripRecord.DepartureDate.GetValueOrDefault(),
                                Segments = new List<FlightSegment>()
                            };
                            tripLookup.Add(tripRecord.Id.GetValueOrDefault(), trip);
                            itinerary.Trips.Add(trip);
                        }
                        FlightSegment segment;
                        if (!segmentLookup.TryGetValue(segmentRecord.Id.GetValueOrDefault(), out segment))
                        {
                            segment = new FlightSegment
                            {
                                OperatingAirlineCode = segmentRecord.OperatingAirlineCd,
                                OperatingAirlineName = GetAirlineName(segmentRecord.OperatingAirlineCd),
                                OperatingAirlineLogoUrl = GetAirlineLogoUrl(segmentRecord.OperatingAirlineCd),
                                AirlineCode = segmentRecord.AirlineCd,
                                AirlineName = GetAirlineName(segmentRecord.AirlineCd),
                                AirlineLogoUrl = GetAirlineLogoUrl(segmentRecord.AirlineCd),
                                FlightNumber = segmentRecord.FlightNumber,
                                DepartureAirport = segmentRecord.DepartureAirportCd,
                                DepartureAirportName = GetAirportName(segmentRecord.DepartureAirportCd),
                                DepartureCity = GetAirportCity(segmentRecord.DepartureAirportCd),
                                DepartureTerminal = segmentRecord.DepartureTerminal,
                                DepartureTime = segmentRecord.DepartureTime.GetValueOrDefault(),
                                ArrivalAirport = segmentRecord.ArrivalAirportCd,
                                ArrivalAirportName = GetAirportName(segmentRecord.ArrivalAirportCd),
                                ArrivalCity = GetAirportCity(segmentRecord.ArrivalAirportCd),
                                ArrivalTerminal = segmentRecord.ArrivalTerminal,
                                ArrivalTime = segmentRecord.ArrivalTime.GetValueOrDefault(),
                                CabinClass = CabinClassCd.Mnemonic(segmentRecord.CabinClassCd),
                                Baggage = segmentRecord.Baggage
                            };
                            segmentLookup.Add(segmentRecord.Id.GetValueOrDefault(), segment);
                            trip.Segments.Add(segment);
                        }
                        FlightPassenger passenger;
                        if (
                            !passengerLookup.TryGetValue(passengerRecord.Id.GetValueOrDefault(),
                                out passenger))
                        {
                            passenger = new FlightPassenger
                            {
                                Title = TitleCd.Mnemonic(passengerRecord.TitleCd),
                                FirstName = passengerRecord.FirstName,
                                LastName = passengerRecord.LastName,
                                Type = PassengerTypeCd.Mnemonic(passengerRecord.TypeCd)
                            };
                            reservation.Passengers.Add(passenger);
                            passengerLookup.Add(passengerRecord.Id.GetValueOrDefault(), passenger);
                        }
                        return reservation;
                    }).Distinct().Single();
                return reservation;
            }
        }

        internal FlightReservation GetReservationFromDb(string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                FlightReservation reservation = null;
                var itineraryLookup = new Dictionary<long, FlightItinerary>();
                var tripLookup = new Dictionary<long, FlightTrip>();
                var segmentLookup = new Dictionary<long, FlightSegment>();
                var stopLookup = new Dictionary<long, FlightStop>();
                var passengerLookup = new Dictionary<long, FlightPassenger>();
                reservation = GetReservationQuery.GetInstance().ExecuteMultiMap(conn, new { RsvNo = rsvNo },
                    (reservationRecord, itineraryRecord, tripRecord, segmentRecord, passengerRecord, stopRecord) =>
                    {
                        if (reservationRecord == null)
                            return reservation;
                        if (reservation == null)
                        {
                            reservation = new FlightReservation
                            {
                                RsvNo = rsvNo,
                                RsvTime = reservationRecord.RsvTime.GetValueOrDefault(),
                                Contact = Contact.GetFromDb(rsvNo),
                                Payment = PaymentDetails.GetFromDb(rsvNo),
                                OverallTripType = TripTypeCd.Mnemonic(reservationRecord.OverallTripTypeCd),
                                Itineraries = new List<FlightItinerary>(),
                                Passengers = new List<FlightPassenger>(),
                                State = ReservationState.GetFromDb(rsvNo),
                                User = User.GetFromDb(reservationRecord.UserId)
                            };
                        }
                        FlightItinerary itinerary;
                        if (
                            !itineraryLookup.TryGetValue(itineraryRecord.Id.GetValueOrDefault(),
                                out itinerary))
                        {
                            itinerary = new FlightItinerary
                            {
                                BookingId = itineraryRecord.BookingId,
                                BookingStatus = BookingStatusCd.Mnemonic(itineraryRecord.BookingStatusCd),
                                TripType = TripTypeCd.Mnemonic(itineraryRecord.TripTypeCd),
                                Supplier = SupplierCd.Mnemonic(itineraryRecord.SupplierCd),
                                TimeLimit = itineraryRecord.TicketTimeLimit,
                                Trips = new List<FlightTrip>(),
                                Price = Price.GetFromDb(itineraryRecord.Id.GetValueOrDefault()),
                                FareType = FareTypeCd.Mnemonic(itineraryRecord.FareTypeCd),
                            };
                            itineraryLookup.Add(itineraryRecord.Id.GetValueOrDefault(), itinerary);
                            reservation.Itineraries.Add(itinerary);
                        }
                        FlightTrip trip;
                        if (!tripLookup.TryGetValue(tripRecord.Id.GetValueOrDefault(), out trip))
                        {
                            trip = new FlightTrip
                            {
                                OriginAirport = tripRecord.OriginAirportCd,
                                OriginAirportName = GetAirportName(tripRecord.OriginAirportCd),
                                OriginCity = GetAirportCity(tripRecord.OriginAirportCd),
                                DestinationAirport = tripRecord.DestinationAirportCd,
                                DestinationAirportName = GetAirportName(tripRecord.DestinationAirportCd),
                                DestinationCity = GetAirportCity(tripRecord.DestinationAirportCd),
                                DepartureDate = tripRecord.DepartureDate.GetValueOrDefault(),
                                Segments = new List<FlightSegment>()
                            };
                            tripLookup.Add(tripRecord.Id.GetValueOrDefault(), trip);
                            itinerary.Trips.Add(trip);
                        }
                        FlightSegment segment;
                        if (!segmentLookup.TryGetValue(segmentRecord.Id.GetValueOrDefault(), out segment))
                        {
                            segment = new FlightSegment
                            {
                                OperatingAirlineCode = segmentRecord.OperatingAirlineCd,
                                OperatingAirlineName = GetAirlineName(segmentRecord.OperatingAirlineCd),
                                OperatingAirlineLogoUrl = GetAirlineLogoUrl(segmentRecord.OperatingAirlineCd),
                                AirlineCode = segmentRecord.AirlineCd,
                                AirlineName = GetAirlineName(segmentRecord.AirlineCd),
                                AirlineLogoUrl = GetAirlineLogoUrl(segmentRecord.AirlineCd),
                                FlightNumber = segmentRecord.FlightNumber,
                                DepartureAirport = segmentRecord.DepartureAirportCd,
                                DepartureAirportName = GetAirportName(segmentRecord.DepartureAirportCd),
                                DepartureCity = GetAirportCity(segmentRecord.DepartureAirportCd),
                                DepartureTerminal = segmentRecord.DepartureTerminal,
                                DepartureTime = segmentRecord.DepartureTime.GetValueOrDefault(),
                                ArrivalAirport = segmentRecord.ArrivalAirportCd,
                                ArrivalAirportName = GetAirportName(segmentRecord.ArrivalAirportCd),
                                ArrivalCity = GetAirportCity(segmentRecord.ArrivalAirportCd),
                                ArrivalTerminal = segmentRecord.ArrivalTerminal,
                                ArrivalTime = segmentRecord.ArrivalTime.GetValueOrDefault(),
                                Duration = segmentRecord.Duration.GetValueOrDefault(),
                                CabinClass = CabinClassCd.Mnemonic(segmentRecord.CabinClassCd),
                                Baggage = segmentRecord.Baggage,
                                Pnr = segmentRecord.Pnr
                            };
                            segmentLookup.Add(segmentRecord.Id.GetValueOrDefault(), segment);
                            trip.Segments.Add(segment);
                        }
                        FlightStop stop;
                        if (stopRecord != null && !stopLookup.TryGetValue(stopRecord.Id.GetValueOrDefault(), out stop))
                        {
                            stop = new FlightStop
                            {
                                Airport = stopRecord.AirportCd,
                                DepartureTime = stopRecord.DepartureTime.GetValueOrDefault(),
                                ArrivalTime = stopRecord.ArrivalTime.GetValueOrDefault(),
                                Duration = stopRecord.Duration.GetValueOrDefault()
                            };
                            stopLookup.Add(stopRecord.Id.GetValueOrDefault(), stop);
                            segment.Stops.Add(stop);
                            segment.StopQuantity++;
                        }
                        FlightPassenger passenger;
                        if (!passengerLookup.TryGetValue(passengerRecord.Id.GetValueOrDefault(), out passenger))
                        {
                            passenger = new FlightPassenger
                            {
                                Title = TitleCd.Mnemonic(passengerRecord.TitleCd),
                                FirstName = passengerRecord.FirstName,
                                LastName = passengerRecord.LastName,
                                Type = PassengerTypeCd.Mnemonic(passengerRecord.TypeCd),
                                DateOfBirth = passengerRecord.BirthDate,
                                Gender = GenderCd.Mnemonic(passengerRecord.GenderCd),
                                Nationality = passengerRecord.NationalityCd,
                                PassportNumber = passengerRecord.PassportNumber,
                                PassportCountry = passengerRecord.PassportCountryCd,
                                PassportExpiryDate = passengerRecord.PassportExpiryDate
                            };
                            reservation.Passengers.Add(passenger);
                            passengerLookup.Add(passengerRecord.Id.GetValueOrDefault(), passenger);
                        }
                        return reservation;
                    }).Distinct().Single();
                return reservation;
            }
        }

        private static List<FlightMarginRule> GetActivePriceMarginRulesFromDb()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var activeRules = GetActivePriceMarginRuleQuery.GetInstance().ExecuteMultiMap(conn, null, null,
                    (marginRecord, ruleRecord) =>
                    {
                        var margin = new Margin
                        {
                            Id = marginRecord.Id.GetValueOrDefault(),
                            RuleId = marginRecord.OrderRuleId.GetValueOrDefault(),
                            Name = marginRecord.Name,
                            Description = marginRecord.Description,
                            Percentage = marginRecord.Percentage.GetValueOrDefault(),
                            Constant = marginRecord.Constant.GetValueOrDefault(),
                            Currency = new Currency(marginRecord.CurrencyCd),
                            IsFlat = marginRecord.IsFlat.GetValueOrDefault(),
                            IsActive = marginRecord.IsActive.GetValueOrDefault(),
                        };
                        var rule = new FlightItineraryRule
                        {
                            BookingDateSpans = ruleRecord.BookingDateSpans.Deserialize<List<DateSpanRule>>(),
                            BookingDays = ruleRecord.BookingDays.Deserialize<List<DayOfWeek>>(),
                            BookingDates = ruleRecord.BookingDates.Deserialize<List<DateTime>>(),
                            FareTypes = ruleRecord.FareTypes.Deserialize<List<FareType>>(),
                            AirlineTypes = ruleRecord.AirlineTypes.Deserialize<List<AirlineType>>(),
                            CabinClasses = ruleRecord.CabinClasses.Deserialize<List<CabinClass>>(),
                            TripTypes = ruleRecord.RequestedTripTypes.Deserialize<List<TripType>>(),
                            DepartureDateSpans = ruleRecord.DepartureDateSpans.Deserialize<List<DateSpanRule>>(),
                            DepartureDays = ruleRecord.DepartureDays.Deserialize<List<DayOfWeek>>(),
                            DepartureDates = ruleRecord.DepartureDates.Deserialize<List<DateTime>>(),
                            DepartureTimeSpans = ruleRecord.DepartureTimeSpans.Deserialize<List<TimeSpanRule>>(),
                            ReturnDateSpans = ruleRecord.ReturnDateSpans.Deserialize<List<DateSpanRule>>(),
                            ReturnDays = ruleRecord.ReturnDays.Deserialize<List<DayOfWeek>>(),
                            ReturnDates = ruleRecord.ReturnDates.Deserialize<List<DateTime>>(),
                            ReturnTimeSpans = ruleRecord.ReturnTimeSpans.Deserialize<List<TimeSpanRule>>(),
                            MaxPassengers = ruleRecord.MaxPassengers.GetValueOrDefault(),
                            MinPassengers = ruleRecord.MinPassengers.GetValueOrDefault(),
                            Airlines = ruleRecord.Airlines.Deserialize<List<string>>(),
                            AirlinesIsExclusion = ruleRecord.AirlinesIsExclusion.GetValueOrDefault(),
                            AirportPairs = ruleRecord.AirportPairs.Deserialize<List<AirportPairRule>>(),
                            AirportPairsIsExclusion = ruleRecord.AirportPairsIsExclusion.GetValueOrDefault(),
                            CityPairs = ruleRecord.CityPairs.Deserialize<List<AirportPairRule>>(),
                            CityPairsIsExclusion = ruleRecord.CityPairsIsExclusion.GetValueOrDefault(),
                            CountryPairs = ruleRecord.CountryPairs.Deserialize<List<AirportPairRule>>(),
                            CountryPairsIsExclusion = ruleRecord.CountryPairsIsExclusion.GetValueOrDefault(),
                            ConstraintCount = ruleRecord.ConstraintCount.GetValueOrDefault(),
                            Priority = ruleRecord.Priority.GetValueOrDefault(),
                        };
                        return new FlightMarginRule(margin, rule);
                    }, "BookingDateSpans").ToList();
                return activeRules.ToList();
            }
        }

        internal static List<string> GetRsvNoByBookingIdFromDb(List<string> bookingIds)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                return GetRsvNoByBookingIdQuery.GetInstance().Execute(conn, new { BookingIds = bookingIds }).Distinct().ToList();
            }
        }

        private static List<FlightPassenger> GetSavedPassengersFromDb(string contactEmail)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var passengerRecords =
                    GetSavedPassengersByContactEmailQuery.GetInstance().Execute(conn, new { ContactEmail = contactEmail }).ToList();
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
        #endregion

        #region Insert
        private static void InsertReservationToDb(FlightReservation reservation)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservationRecord = new FlightReservationTableRecord
                {
                    RsvNo = reservation.RsvNo,
                    RsvTime = reservation.RsvTime.ToUniversalTime(),
                    RsvStatusCd = RsvStatusCd.Mnemonic(reservation.RsvStatus),
                    CancellationTypeCd = null,
                    AdultCount = reservation.Passengers.Count(p => p.Type == PassengerType.Adult),
                    ChildCount = reservation.Passengers.Count(p => p.Type == PassengerType.Child),
                    InfantCount = reservation.Passengers.Count(p => p.Type == PassengerType.Infant),
                    OverallTripTypeCd = TripTypeCd.Mnemonic(reservation.OverallTripType),
                    UserId = reservation.User != null ? reservation.User.Id : null,
                    InsertBy = "LunggoSystem",
                    InsertDate = DateTime.UtcNow,
                    InsertPgId = "0"
                };

                FlightReservationTableRepo.GetInstance().Insert(conn, reservationRecord);
                reservation.Contact.InsertToDb(reservation.RsvNo);
                reservation.State.InsertToDb(reservation.RsvNo);
                reservation.Payment.InsertToDb(reservation.RsvNo);

                foreach (var itin in reservation.Itineraries)
                {
                    var itineraryId = FlightItineraryIdSequence.GetInstance().GetNext();
                    var itineraryRecord = new FlightItineraryTableRecord
                    {
                        Id = itineraryId,
                        RsvNo = reservation.RsvNo,
                        BookingId = itin.BookingId,
                        BookingStatusCd = BookingStatusCd.Mnemonic(BookingStatus.Booked),
                        TicketTimeLimit = itin.TimeLimit,
                        FareTypeCd = FareTypeCd.Mnemonic(itin.FareType),
                        SupplierCd = SupplierCd.Mnemonic(itin.Supplier),
                        TripTypeCd = TripTypeCd.Mnemonic(itin.TripType),
                        InsertBy = "LunggoSystem",
                        InsertDate = DateTime.UtcNow,
                        InsertPgId = "0",
                    };
                    FlightItineraryTableRepo.GetInstance().Insert(conn, itineraryRecord);
                    itin.Price.InsertToDb(itineraryId);

                    foreach (var trip in itin.Trips)
                    {
                        var tripId = FlightTripIdSequence.GetInstance().GetNext();
                        var tripRecord = new FlightTripTableRecord
                        {
                            Id = tripId,
                            ItineraryId = itineraryId,
                            OriginAirportCd = trip.OriginAirport,
                            DestinationAirportCd = trip.DestinationAirport,
                            DepartureDate = trip.DepartureDate.ToUniversalTime(),
                            InsertBy = "LunggoSystem",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "0"
                        };
                        FlightTripTableRepo.GetInstance().Insert(conn, tripRecord);

                        var segments = trip.Segments;
                        foreach (var segment in segments)
                        {
                            var segmentId = FlightSegmentIdSequence.GetInstance().GetNext();
                            var segmentRecord = new FlightSegmentTableRecord
                            {
                                Id = segmentId,
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
                                AirlineTypeCd = AirlineTypeCd.Mnemonic(segment.AirlineType),
                                Meal = segment.Meal,
                                Rbd = segment.Rbd,
                                InsertBy = "LunggoSystem",
                                InsertDate = DateTime.UtcNow,
                                InsertPgId = "0"
                            };
                            FlightSegmentTableRepo.GetInstance().Insert(conn, segmentRecord);

                            if (segment.StopQuantity > 0)
                            {
                                foreach (var stop in segment.Stops)
                                {
                                    var stopId = FlightStopIdSequence.GetInstance().GetNext();
                                    var stopRecord = new FlightStopTableRecord
                                    {
                                        Id = stopId,
                                        SegmentId = segmentId,
                                        ArrivalTime = stop.ArrivalTime.ToUniversalTime(),
                                        DepartureTime = stop.DepartureTime.ToUniversalTime(),
                                        Duration = stop.Duration,
                                        AirportCd = stop.Airport,
                                        InsertBy = "LunggoSystem",
                                        InsertDate = DateTime.UtcNow,
                                        InsertPgId = "0"
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
                        Id = FlightPassengerIdSequence.GetInstance().GetNext(),
                        RsvNo = reservation.RsvNo,
                        TypeCd = PassengerTypeCd.Mnemonic(passenger.Type),
                        GenderCd = GenderCd.Mnemonic(passenger.Gender),
                        TitleCd = TitleCd.Mnemonic(passenger.Title),
                        FirstName = passenger.FirstName,
                        LastName = passenger.LastName,
                        BirthDate = passenger.DateOfBirth.HasValue ? passenger.DateOfBirth.Value.ToUniversalTime() : (DateTime?)null,
                        NationalityCd = passenger.Nationality,
                        PassportNumber = passenger.PassportNumber,
                        PassportExpiryDate = passenger.PassportExpiryDate.HasValue ? passenger.PassportExpiryDate.Value.ToUniversalTime() : (DateTime?)null,
                        PassportCountryCd = passenger.PassportCountry,
                        InsertBy = "LunggoSystem",
                        InsertDate = DateTime.UtcNow,
                        InsertPgId = "0"
                    };
                    FlightPassengerTableRepo.GetInstance().Insert(conn, passengerRecord);
                }

            }
        }

        private static void InsertDetailsToDb(GetTripDetailsResult details)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var itineraryId = GetItineraryIdQuery.GetInstance().Execute(conn, new { details.BookingId }).Single();

                foreach (var trip in details.Itinerary.Trips)
                {
                    var tripId = FlightTripIdSequence.GetInstance().GetNext();
                    var tripRecord = new FlightTripTableRecord
                    {
                        Id = tripId,
                        ItineraryId = itineraryId,
                        OriginAirportCd = trip.OriginAirport,
                        DestinationAirportCd = trip.DestinationAirport,
                        DepartureDate = trip.DepartureDate.ToUniversalTime(),
                        InsertBy = "LunggoSystem",
                        InsertDate = DateTime.UtcNow,
                        InsertPgId = "0"
                    };
                    FlightTripTableRepo.GetInstance().Insert(conn, tripRecord);

                    var segments = trip.Segments;
                    foreach (var segment in segments)
                    {
                        var segmentId = FlightSegmentIdSequence.GetInstance().GetNext();
                        var segmentRecord = new FlightSegmentTableRecord
                        {
                            Id = segmentId,
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
                            InsertBy = "LunggoSystem",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "0"
                        };
                        FlightSegmentTableRepo.GetInstance().Insert(conn, segmentRecord);
                    }
                }
            }
        }

        private static void InsertSavedPassengersToDb(string contactEmail, List<FlightPassenger> passengers)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var savedPassengers = GetSavedPassengersByContactEmailQuery.GetInstance()
                    .Execute(conn, new { ContactEmail = contactEmail }).ToList();
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

        private static void InsertPriceMarginRulesToDb(List<FlightMarginRule> rules, List<FlightMarginRule> deletedRules)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var activeRules = GetActivePriceMarginRulesFromDb();
                foreach (var deletedRule in deletedRules)
                {
                    if (activeRules.Any(activeRule => activeRule.Margin.Id == deletedRule.Margin.Id))
                    {
                        FlightItineraryRuleTableRepo.GetInstance()
                            .Update(conn, new FlightItineraryRuleTableRecord
                            {
                                Id = deletedRule.Margin.RuleId,
                                Priority = deletedRule.Rule.Priority,
                            });
                        MarginTableRepo.GetInstance()
                            .Update(conn, new MarginTableRecord
                            {
                                Id = deletedRule.Margin.Id,
                                IsActive = deletedRule.Margin.IsActive
                            });
                    }
                }
                foreach (var marginRule in rules)
                {
                    if (activeRules.Any(activeRule => activeRule.Margin.RuleId == marginRule.Margin.RuleId))
                    {
                        FlightItineraryRuleTableRepo.GetInstance()
                            .Update(conn, new FlightItineraryRuleTableRecord
                            {
                                Id = marginRule.Margin.RuleId,
                                Priority = marginRule.Rule.Priority,
                            });
                    }
                    else
                    {
                        var margin = marginRule.Margin;
                        var marginRecord = new MarginTableRecord
                        {
                            Id = margin.Id,
                            Name = margin.Name,
                            Description = margin.Description,
                            Percentage = margin.Percentage,
                            Constant = margin.Constant,
                            IsFlat = margin.IsFlat,
                            IsActive = true,
                            InsertBy = "LunggoSystem",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "0"
                        };
                        MarginTableRepo.GetInstance().Insert(conn, marginRecord);

                        var rule = marginRule.Rule;
                        var ruleRecord = new FlightItineraryRuleTableRecord
                        {
                            Id = marginRule.Margin.RuleId,
                            BookingDateSpans = rule.BookingDateSpans.Serialize(),
                            BookingDays = rule.BookingDays.Serialize(),
                            BookingDates = rule.BookingDates.Serialize(),
                            FareTypes = rule.FareTypes.Serialize(),
                            CabinClasses = rule.CabinClasses.Serialize(),
                            RequestedTripTypes = rule.TripTypes.Serialize(),
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
                            ConstraintCount = rule.ConstraintCount,
                            Priority = rule.Priority,
                            InsertBy = "LunggoSystem",
                            InsertDate = DateTime.UtcNow,
                            InsertPgId = "0"
                        };
                        FlightItineraryRuleTableRepo.GetInstance().Insert(conn, ruleRecord);
                    }
                }
            }
        }
        #endregion

        #region Update
        private static void UpdateDetailsToDb(GetTripDetailsResult details)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                foreach (var segment in details.Itinerary.Trips.SelectMany(trip => trip.Segments))
                {
                    UpdateDetailsQuery.GetInstance().Execute(conn, new
                    {
                        details.BookingId,
                        segment.DepartureAirport,
                        segment.ArrivalAirport,
                        segment.DepartureTime,
                        segment.ArrivalTime,
                        segment.Duration,
                        segment.Pnr,
                        segment.DepartureTerminal,
                        segment.ArrivalTerminal,
                        segment.Baggage
                    });
                }
            }
        }

        private static void UpdateCancelReservationToDb(string rsvNo, CancellationType type)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var cancellationType = CancellationTypeCd.Mnemonic(type);
                CancelReservationsQuery.GetInstance()
                    .Execute(conn, new { RsvNo = rsvNo, CancellationType = cancellationType });
            }
        }

        private static void UpdateExpireReservationsToDb()
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var minuteTimeout = int.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "paymentTimeout"));
                ExpireReservationsQuery.GetInstance().Execute(conn, new { MinuteTimeout = minuteTimeout });
            }
        }

        private static void UpdateBookingStatusToDb(List<BookingStatusInfo> statusData)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var query = UpdateBookingStatusQuery.GetInstance();
                var dbBookingStatusInfo = statusData.Select(info => new
                {
                    info.BookingId,
                    BookingStatusCd = BookingStatusCd.Mnemonic(info.BookingStatus)
                }).ToArray();
                query.Execute(conn, dbBookingStatusInfo);
            }
        }

        //private static void UpdateIssueProgressToDb(string rsvNo, string progressMessage)
        //{
        //    using (var conn = DbService.GetInstance().GetOpenConnection())
        //    {
        //        FlightReservationTableRepo.GetInstance().Update(conn, new FlightReservationTableRecord
        //        {
        //            RsvNo = rsvNo,
        //            IssueProgress = progressMessage
        //        });
        //    }
        //}
        #endregion
    }
}
