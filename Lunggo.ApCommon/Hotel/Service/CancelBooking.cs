using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds;

namespace Lunggo.ApCommon.Hotel.Service
{
    public partial class HotelService
    {
        public HotelCancelBookingOutput CancelHotelBooking(HotelCancelBookingInput input)
        {
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

            var request = new HotelCancelBookingInfo
            {
                BookingReference = rsv.HotelDetails.BookingReference
            };
            var hotelbedsClient = new HotelBedsCancelBooking();
            var cancelBookingResponse = hotelbedsClient.CancelHotelBooking(request);
            
            //Get Booking Reference at first
            //Check if exist or not
            //if exist, check if can refundable or not
            //call cancel hotelbeds api

            return new HotelCancelBookingOutput();
        }
    }

}
