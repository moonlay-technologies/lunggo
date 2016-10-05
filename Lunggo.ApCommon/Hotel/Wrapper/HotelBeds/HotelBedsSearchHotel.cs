using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.helpers;
using Newtonsoft.Json;


namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds
{
    public class HotelBedsSearchHotel
    {
        public SearchHotelResult SearchHotel(SearchHotelCondition condition)
        {
            HotelApiClient client = new HotelApiClient("p8zy585gmgtkjvvecb982azn", "QrwuWTNf8a", "blabla");
            //HotelApiClient client = new HotelApiClient();
            var avail = new Availability
            {
                checkIn = condition.CheckIn,
                checkOut =  condition.Checkout,
                destination = condition.Location,
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
            for (int i = 0; i < condition.AdultCount; i++)
            {
            room.adultOf(30);
            }

            for (int i = 0; i < condition.ChildCount; i++)
            {
                room.childOf(30);
            }
            avail.rooms.Add(room);
            AvailabilityRQ availabilityRq = avail.toAvailabilityRQ();
                if (availabilityRq == null)
                    throw new Exception("Availability RQ can't be null", new ArgumentNullException());

                Console.WriteLine(JsonConvert.SerializeObject(availabilityRq, Formatting.Indented, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore }));
                AvailabilityRS responseAvail = client.doAvailability(availabilityRq);

            var response = new SearchHotelResult();
            List<HotelRoom> rooms = new List<HotelRoom>();
            
            //var hotels = new HotelDetail();

            if (responseAvail != null && responseAvail.hotels != null && responseAvail.hotels.hotels != null &&
                responseAvail.hotels.hotels.Count > 0)
            {
                foreach (var hotelResponse in responseAvail.hotels.hotels)
                {
                    var hotel = new HotelDetail()
                    {
                        HotelCode = hotelResponse.code,
                        HotelName = hotelResponse.name,
                        //Address = hotelResponse.address,
                        Latitude = double.Parse(hotelResponse.latitude),
                        Longitude = double.Parse(hotelResponse.longitude),
                        ZoneCode =  hotelResponse.zoneCode,
                        NetFare =  hotelResponse.totalNet,
                        OriginalFare = hotelResponse.totalSellingRate,
                        Review = hotelResponse.reviews,
                        Rooms =hotelResponse.rooms.Select(roomAPI=>new HotelRoom
                        {
                            RoomCode = roomAPI.code
                        }).ToList()
                    };
                }
            }
            return response;
        }
    }
}
