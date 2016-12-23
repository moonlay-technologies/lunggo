using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;
using Lunggo.ApCommon.Product.Constant;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public HotelCancelBookingOutput CancelHotelBooking(HotelCancelBookingInput input)
        {
            var cancellationDate = DateTime.UtcNow;
            var rsv = GetReservation(input.BookingId);
            if (rsv == null)
            {
                return new HotelCancelBookingOutput
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> {"Reservation not found"}
                };
            }

            if (string.IsNullOrEmpty(rsv.HotelDetails.BookingReference))
            {
                return new HotelCancelBookingOutput
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Booking Reference  not found" }
                };
            }

            if (rsv.RsvStatus != RsvStatus.Completed)
            {
                return new HotelCancelBookingOutput
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Cannot Cancel the book because the status is not complete yet" }
                };
            }

            var request = new HotelCancelBookingInfo
            {
                BookingReference = rsv.HotelDetails.BookingReference
            };
            var hotelbedsClient = new HotelBedsCancelBooking();
            var cancelBookingResponse = hotelbedsClient.CancelHotelBooking(request);
            if (cancelBookingResponse.status != "SUCCESS")
            {
                return new HotelCancelBookingOutput
                {
                    IsSuccess = false,
                    ErrorMessages = new List<string> {"Failed from the Supplier"}
                };
            }
            //TODO Remember to check this code to update database
            UpdateCancellationStatusDb(rsv.RsvNo, RsvStatus.Cancelled, cancellationDate, cancelBookingResponse.CancellationReference);

            //Update Reservation : Status, Cancellation Time
            return new HotelCancelBookingOutput
            {
                BookingId = rsv.RsvNo,
                CancellationAmount = cancelBookingResponse.CancellationAmount,
                CancellationDate = cancellationDate,
                IsSuccess = true
            };
        }
    }

}
