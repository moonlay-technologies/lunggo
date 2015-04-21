using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Query.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class InsertFlightDb
    {
        internal static void Booking(FlightBookingRecord bookingRecord)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var rsvNo = "rsvNo";
                var reservationRecord = new FlightReservationTableRecord
                {
                    RsvNo = rsvNo,
                    RsvStatusCd = "xxx",
                    RsvTime = DateTime.Now,
                    ContactName = bookingRecord.ContactData.Name,
                    ContactEmail = bookingRecord.ContactData.Email,
                    ContactPhone = bookingRecord.ContactData.Phone,
                    LangCd = "xxx",
                    MemberCd = "xxx",
                    CancellationTypeCd = "xxx",
                    OverallTripTypeCd = TripTypeCd.Mnemonic(bookingRecord.OverallTripType),
                    FinalPrice = 999,
                    PaymentFeeForCust = 999,
                    PaymentFeeForUs = 999,
                    TotalSourcePrice = 999,
                    PaymentMethodCd = "xxx",
                    PaymentStatusCd = "xxx",
                    GrossProfit = 999,
                    InsertBy = "xxx",
                    InsertDate = DateTime.Now,
                    InsertPgId = "xxx"
                };

                FlightReservationTableRepo.GetInstance().Insert(conn, reservationRecord);

                foreach (var record in bookingRecord.ItineraryRecords)
                {
                    var itineraryId = 999;
                    var itineraryRecord = new FlightItineraryTableRecord
                    {
                        ItineraryId = itineraryId,
                        RsvNo = rsvNo,
                        BookingId = record.BookResult.BookingId,
                        BookingStatusCd = BookingStatusCd.Mnemonic(BookingStatus.Booked),
                        SupplierCd = FlightSupplierCd.Mnemonic(FlightSupplier.Mystifly),
                        SupplierPrice = 999,
                        SupplierCurrencyCd = "xxx",
                        SupplierIdrExchangeRate = 999,
                        IdrPrice = 999,
                        LocalPrice = 999,
                        LocalCurrencyCd = "xxx",
                        LocalIdrExchangeRate = 999,
                        TripTypeCd = TripTypeCd.Mnemonic(TripType.OneWay),
                        InsertBy = "xxx",
                        InsertDate = DateTime.Now,
                        InsertPgId = "xxx"
                    };
                    FlightItineraryTableRepo.GetInstance().Insert(conn, itineraryRecord);

                    var i = 0;
                    foreach (var trip in record.Itinerary.FlightTrips)
                    {
                        var tripId = i;
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
                        do
                        {
                            var segmentId = i + 10;
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
                                    var stopId = i + 100;
                                    var stopRecord = new FlightStopTableRecord
                                    {
                                        StopId = stopId,
                                        SegmentId = segmentId,
                                        ArrivalTime = stop.Arrival,
                                        DepartureTime = stop.Departure,
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
                        } while (orderedSegments[i].ArrivalAirport != trip.DestinationAirport);
                    }

                    foreach (var passenger in bookingRecord.Passengers)
                    {

                        var passengerRecord = PassengerBookingRecord(passenger, rsvNo);
                        FlightPassengerTableRepo.GetInstance().Insert(conn, passengerRecord);
                    }
                }
            }
        }

        internal static void Details(FlightDetailsRecord detailsRecord)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var segmentRecords = GetFlightSegment.GetInstance().Execute(conn, new { detailsRecord.BookingId });
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
                    var passengerPrimKey = GetFlightPassengerPrimKey.GetInstance().Execute(conn, new
                    {
                        passenger.FirstName,
                        passenger.LastName,
                        passenger.DateOfBirth,
                        passenger.IdNumber
                    });
                    foreach (var eticket in passenger.ETicket)
                    {
                        var eticketId = 999;
                        var referencedSegment =
                            detailsRecord.Segments.Single(segment => segment.Reference == eticket.Reference);
                        //var referencedRecord = segmentRecords.Single(segment => segment.DepartureAirportCd)
                        var record = new FlightEticketTableRecord
                        {
                            EticketId = eticketId,
                            

                        };
                    }
                }
                
            }
        }

        private static FlightPassengerTableRecord PassengerBookingRecord(PassengerFareInfo passenger, string rsvNo)
        {
            return new FlightPassengerTableRecord
            {
                PassengerId = 999,
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
            };
        }
    }
}
