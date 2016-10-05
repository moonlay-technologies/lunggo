using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.hotelbeds.distribution.hotel_api_model.auto.model;
using com.hotelbeds.distribution.hotel_api_sdk.helpers;
using Lunggo.ApCommon.Hotel.Model;
using com.hotelbeds.distribution.hotel_api_sdk;
using com.hotelbeds.distribution.hotel_api_model;


namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds
{
    internal partial class HotelBedsSearchHotel
    {
        internal SearchHotelResult SearchHotel(SearchHotelCondition condition)
        {
            HotelApiClient client = new HotelApiClient("p8zy585gmgtkjvvecb982azn", "QrwuWTNf8a", "blabla");
            //HotelApiClient client = new HotelApiClient();
            var avail = new Availability
            {
                checkIn = condition.CheckIn,
                checkOut =  condition.Checkout,
                destination = condition.Location, //not good yet
                language = "ENG",
                zone =  condition.Zone,
                payed = Availability.Pay.AT_WEB
            };
            AvailRoom room = new AvailRoom
            {
                adults = condition.AdultCount,
                children = condition.ChildCount,
                //Room Detail
                numberOfRooms = condition.Rooms
            };
            room.details = new List<RoomDetail>();
            room.adultOf(30);
            room.adultOf(30);

            return new SearchHotelResult();
        }
    }
}
