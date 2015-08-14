using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Logic;
using Lunggo.ApCommon.Flight.Database.Model;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public BookFlightOutput BookFlight(BookFlightInput input)
        {
            var output = new BookFlightOutput();
            var bookInfo = new FlightBookingInfo
            {
                FareId = input.Itinerary.FareId,
                ContactData = input.ContactData,
                PassengerInfoFares = input.PassengerInfoFares
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
                var bookingRecord = new FlightBookingRecord
                {
                    OverallTripType = input.OverallTripType,
                    ItineraryRecords = new List<FlightBookingItineraryRecord>
                    {
                        new FlightBookingItineraryRecord
                        {
                            Itinerary = input.Itinerary,
                            BookResult = output.BookResult,
                        }
                    },
                    ContactData = input.ContactData,
                    Passengers = input.PassengerInfoFares,
                    DiscountCode = input.DiscountCode
                };
                string rsvNo;
                InsertFlightDb.Booking(bookingRecord, out rsvNo);
                output.RsvNo = rsvNo;
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
    }
}
