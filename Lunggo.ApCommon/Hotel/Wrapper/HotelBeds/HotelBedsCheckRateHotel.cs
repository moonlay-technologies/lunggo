using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.helpers;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds
{
    public class HotelBedsCheckRate
    {
        public RevalidateHotelResult CheckRateHotel(HotelRevalidateInfo hotelRate)
        {
            var client = new HotelApiClient("p8zy585gmgtkjvvecb982azn", "QrwuWTNf8a", "https://api.test.hotelbeds.com/hotel-api");

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
                        checkRateResult.RateKey = responseCheckRate.hotel.rooms[0].rates[0].rateKey;
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
