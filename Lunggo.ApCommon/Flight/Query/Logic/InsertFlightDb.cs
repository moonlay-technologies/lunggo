﻿using System;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
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
                    ContactCountryCode = bookingRecord.ContactData.CountryCode,
                    ContactPhone = bookingRecord.ContactData.Phone,
                    LangCd = "xxx",
                    MemberCd = "xxx",
                    CancellationTypeCd = "xxx",
                    OverallTripTypeCd = TripTypeCd.Mnemonic(bookingRecord.OverallTripType),
                    FinalPrice = bookingRecord.ItineraryRecords[0].Itinerary.IdrPrice,
                    PaymentFeeForCust = 0,
                    PaymentFeeForUs = 0,
                    TotalSupplierPrice = bookingRecord.ItineraryRecords[0].Itinerary.IdrPrice,
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
                        IdrPrice = record.Itinerary.IdrPrice,
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

                        var orderedSegments = trip.FlightSegments.OrderBy(segment => segment.DepartureTime).ToList();
                        var i = 0;
                        do
                        {
                            var segmentId = FlightSegmentIdSequence.GetInstance().GetNext();
                            var segmentRecord = new FlightSegmentTableRecord
                            {
                                SegmentId = segmentId,
                                TripId = tripId,
                                DepartureAirportCd = orderedSegments[i].DepartureAirport,
                                DepartureTime = orderedSegments[i].DepartureTime,
                                ArrivalAirportCd = orderedSegments[i].ArrivalAirport,
                                ArrivalTime = orderedSegments[i].ArrivalTime,
                                AirlineCd = orderedSegments[i].AirlineCode,
                                OperatingAirlineCd = orderedSegments[i].OperatingAirlineCode,
                                FlightNumber = orderedSegments[i].FlightNumber,
                                AircraftCd = orderedSegments[i].AircraftCode,
                                Duration = orderedSegments[i].Duration,
                                StopQuantity = orderedSegments[i].StopQuantity,
                                InsertBy = "xxx",
                                InsertDate = DateTime.Now,
                                InsertPgId = "xxx"
                            };
                            FlightSegmentTableRepo.GetInstance().Insert(conn, segmentRecord);

                            if (orderedSegments[i].StopQuantity > 0)
                            {
                                foreach (var stop in orderedSegments[i].FlightStops)
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

                            i++;
                        } while (i < orderedSegments.Count && orderedSegments[i - 1].ArrivalAirport != trip.DestinationAirport);
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

        internal static void Details(FlightDetailsRecord detailsRecord)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var segmentRecords = GetFlightSegmentQuery.GetInstance().Execute(conn, new { detailsRecord.BookingId }).ToList();
                var segmentPrimKeys = segmentRecords.Select(segment => segment.SegmentId.GetValueOrDefault()).ToList();
                foreach (var segment in detailsRecord.Segments)
                {
                    var record = new FlightDetailsSegmentRecord
                    {
                        BookingId = detailsRecord.BookingId,
                        BookingStatus = BookingStatus.Ticketed,
                        FlightSegmentPrimKeys = segmentPrimKeys,
                        Pnr = segment.Pnr,
                        DepartureTerminal = segment.DepartureTerminal,
                        ArrivalTerminal = segment.ArrivalTerminal,
                        Baggage = segment.Baggage,
                        DepartureAirport = segment.DepartureAirport,
                        ArrivalAirport = segment.ArrivalAirport,
                        DepartureTime = segment.DepartureTime
                    };
                    UpdateFlightDetailsQuery.GetInstance().Execute(conn, record);
                }
                foreach (var passenger in detailsRecord.Passengers)
                {
                    var passengerPrimKey = GetFlightPassengerPrimKeyQuery.GetInstance().Execute(conn, new
                    {
                        passenger.FirstName,
                        passenger.LastName,
                        passenger.DateOfBirth,
                        passenger.IdNumber
                    }).Single();
                    foreach (var eticket in passenger.ETicket)
                    {
                        var eticketId = FlightEticketIdSequence.GetInstance().GetNext();
                        var referencedSegment =
                            detailsRecord.Segments.Single(segment => segment.Reference == eticket.Reference);
                        var referencedRecord = segmentRecords.Single(segment =>
                            segment.DepartureAirportCd == referencedSegment.DepartureAirport &&
                            segment.ArrivalAirportCd == referencedSegment.ArrivalAirport &&
                            segment.DepartureTime == referencedSegment.DepartureTime);
                        var record = new FlightEticketTableRecord
                        {
                            EticketId = eticketId,
                            SegmentId = referencedRecord.SegmentId,
                            PassengerId = passengerPrimKey,
                            EticketNo = eticket.Number,
                            InsertBy = "xxx",
                            InsertDate = DateTime.Now,
                            InsertPgId = "xxx"
                        };
                        FlightEticketTableRepo.GetInstance().Insert(conn, record);
                    }
                }
                
            }
        }
    }
}