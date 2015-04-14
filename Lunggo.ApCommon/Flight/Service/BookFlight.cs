using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Query;
using Lunggo.ApCommon.Flight.Query.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public BookFlightOutput BookFlight(BookFlightInput input)
        {
            var output = new BookFlightOutput();
            var bookInfo = new FlightBookingInfo
            {
                FareId = input.BookingInfo.FareId,
                ContactData = new ContactData
                {
                    Email = input.BookingInfo.ContactData.Email,
                    Phone = input.BookingInfo.ContactData.Phone
                },
                PassengerFareInfos = input.BookingInfo.PassengerFareInfos
            };
            var response = BookFlightInternal(bookInfo);
            output.BookResult = new BookResult();
            if (response.IsSuccess)
            {
                output.IsSuccess = true;
                output.BookResult.BookingId = response.Status.BookingId;
                output.BookResult.BookingStatus = response.Status.BookingStatus;
                if (response.Status.BookingStatus == BookingStatus.Booked)
                    output.BookResult.TimeLimit = response.Status.TimeLimit;
                var flightRecord = new FlightRecord
                {
                    ItineraryRecords = new List<ItineraryRecord>
                    {
                        new ItineraryRecord
                        {
                            Itinerary = input.Itinerary,
                            BookResult = output.BookResult,
                            OriginDestinationInfos = input.OriginDestinationInfos
                        }
                    },
                    ContactData = input.BookingInfo.ContactData,
                    PaymentData = input.PaymentData
                };
                InsertFlightRecordDb(flightRecord);
            }
            else
            {
                output.IsSuccess = false;
                output.BookResult = null;
                output.Errors = response.Errors;
                output.ErrorMessages = response.ErrorMessages;
            }
            return output;
        }

        private static void InsertFlightRecordDb(FlightRecord flightRecord)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var reservationRecord = new FlightReservationQueryRecord
                {
                    ContactData = flightRecord.ContactData,
                    PaymentData = flightRecord.PaymentData,
                    LanguageCode = "ID",
                    RsvNo = "xxx",
                    RsvStatusCode = "GOOD",
                    RsvTime = DateTime.Now,
                    PriceData = new PriceData
                    {
                        TotalSourcePrice = flightRecord.ItineraryRecords.Select(record => record.Itinerary.TotalFare).Sum(),
                        PaymentFeeForCustomer = 100,
                        PaymentFeeForUs = 100,
                        GrossProfit = 100
                    }
                };
                InsertFlightReservationQuery.GetInstance().Execute(conn, reservationRecord);

                foreach (var record in flightRecord.ItineraryRecords)
                {
                    var itineraryRecord = new FlightItineraryQueryRecord
                    {
                        ItineraryId = 100,
                        RsvNo = "xxx",
                        Itinerary = record.Itinerary,
                        TripType = record.Itinerary.TripType,
                        BookResult = record.BookResult,
                        IdrPrice = record.Itinerary.TotalFare,
                        LocalCurrency = "xxx",
                        LocalExchangeRate = 100,
                        LocalPrice = 100,
                        SourceCurrency = "xxx",
                        SourceExchangeRate = 100,
                        SourcePrice = 100
                    };
                    InsertFlightItineraryQuery.GetInstance().Execute(conn, itineraryRecord);

                    var i = 0;
                    foreach (var info in record.OriginDestinationInfos)
                    {
                        var tripRecord = new FlightTripQueryRecord
                        {
                            TripId = 100,
                            ItineraryId = 100,
                            Info = info
                        };
                        InsertFlightTripQuery.GetInstance().Execute(conn, tripRecord);

                        var orderedSegments = record.Itinerary.FlightTrips.OrderBy(trip => trip.DepartureTime).ToList();
                        do
                        {
                            var segmentRecord = new FlightSegmentQueryRecord
                            {
                                SegmentId = 100,
                                TripId = 100,
                                Segment = orderedSegments[i]
                            };
                            InsertFlightSegmentQuery.GetInstance().Execute(conn, segmentRecord);

                            foreach (var stop in orderedSegments[i].FlightStops)
                            {
                                var stopRecord = new FlightStopQueryRecord
                                {
                                    StopId = 100,
                                    SegmentId = 100,
                                    Stop = stop
                                };
                                InsertFlightStopQuery.GetInstance().Execute(conn, stopRecord);
                            }


                            i++;
                        } while (orderedSegments[0].ArrivalAirport != info.DestinationAirport);
                    }

                    foreach (var passenger in flightRecord.Passengers)
                    {
                        var passengerRecord = new FlightPassengerQueryRecord
                        {
                            PassengerId = 100,
                            RsvNo = "xxx",
                            Passenger = passenger
                        };
                        InsertFlightPassengerQuery.GetInstance().Execute(conn, passengerRecord);
                    }
                }
            }
        }
    }
}
