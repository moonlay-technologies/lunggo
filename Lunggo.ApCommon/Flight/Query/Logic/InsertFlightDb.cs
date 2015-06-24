using System;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Query.Model;
using Lunggo.ApCommon.Flight.Utility;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Flight.Query.Logic
{
    internal class InsertFlightDb
    {
        internal static void Booking(FlightBookingRecord bookingRecord, out string rsvNo)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                rsvNo = FlightReservationSequence.GetInstance().GetFlightReservationId(EnumReservationType.ReservationType.NonMember);
                var reservationRecord = new FlightReservationTableRecord
                {
                    RsvNo = rsvNo,
                    RsvStatusCd = "xxx",
                    RsvTime = DateTime.Now,
                    ContactName = bookingRecord.ContactData.Name,
                    ContactEmail = bookingRecord.ContactData.Email,
                    ContactCountryCd = bookingRecord.ContactData.CountryCode,
                    ContactPhone = bookingRecord.ContactData.Phone,
                    LangCd = "xxx",
                    MemberCd = "xxx",
                    CancellationTypeCd = "xxx",
                    OverallTripTypeCd = TripTypeCd.Mnemonic(bookingRecord.OverallTripType),
                    FinalPrice = bookingRecord.ItineraryRecords[0].Itinerary.FinalIdrPrice,
                    PaymentFeeForCust = 0,
                    PaymentFeeForUs = 0,
                    TotalSupplierPrice = bookingRecord.ItineraryRecords[0].Itinerary.FinalIdrPrice,
                    GrossProfit = 0,
                    InsertBy = "xxx",
                    InsertDate = DateTime.Now,
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
                        InsertDate = DateTime.Now,
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
                            InsertDate = DateTime.Now,
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
                                InsertDate = DateTime.Now,
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
                                        InsertDate = DateTime.Now,
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
                            IdNumber = passenger.IdNumber,
                            PassportExpiryDate = passenger.PassportExpiryDate,
                            InsertBy = "xxx",
                            InsertDate = DateTime.Now,
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
                var itineraryId = GetFlightItineraryIdQuery.GetInstance().Execute(conn, details).Single();

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
                        InsertDate = DateTime.Now,
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
                            InsertDate = DateTime.Now,
                            InsertPgId = "xxx"
                        };
                        FlightSegmentTableRepo.GetInstance().Insert(conn, segmentRecord);
                    }
                }
            }
        }
    }
}
