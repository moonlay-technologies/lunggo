using System.Collections.Generic;
using com.hotelbeds.distribution.hotel_api_sdk.helpers;
using com.hotelbeds.distribution.hotel_api_sdk;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds
{
    public class HotelBedsCheckRate
    {
        public RevalidateHotelResult CheckRateHotel(HotelRate hotelRate)
        {
            var client = new HotelApiClient("p8zy585gmgtkjvvecb982azn", "QrwuWTNf8a", "blabla");

            var confirmRoom = new ConfirmRoom {details = new List<RoomDetail>()};

            var bookingCheck = new BookingCheck();
            bookingCheck.addRoom(hotelRate.RateKey, confirmRoom);
            var checkRateRq = bookingCheck.toCheckRateRQ();
            var checkRateResult = new RevalidateHotelResult();

            if (checkRateRq != null)
            {
                var responseCheckRate = client.doCheck(checkRateRq);

                if (responseCheckRate != null && responseCheckRate.error == null)
                {
                    if (responseCheckRate.hotel.rooms[0].rates[0].net == hotelRate.Price)
                    {
                        checkRateResult.IsPriceChanged = false;
                    }
                    else
                    {
                        checkRateResult.IsPriceChanged = true;
                        checkRateResult.NewPrice = responseCheckRate.hotel.rooms[0].rates[0].net;
                    }
                }
                checkRateResult.IsValid = true;
            }
            else
            {
                checkRateResult.IsValid = false;
            }
            return checkRateResult;
        }
    }
}
