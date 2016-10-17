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
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;


namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds
{
    public class HotelBedsSearchHotel
    {
        public SearchHotelResult SearchHotel(SearchHotelCondition condition)
        {
            HotelApiClient client = new HotelApiClient("p8zy585gmgtkjvvecb982azn", "QrwuWTNf8a", "https://api.test.hotelbeds.com/hotel-api");
            var avail = new Availability
            {
                checkIn = condition.CheckIn,
                checkOut =  condition.Checkout,
                destination = condition.Destination ?? null,
                zone = condition.Zone,
                //country belum ada
                language = "ENG",
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
                room.childOf(8);
            }
            avail.rooms.Add(room);
            AvailabilityRQ availabilityRq = avail.toAvailabilityRQ();
                if (availabilityRq == null)
                    throw new Exception("Availability RQ can't be null", new ArgumentNullException());

                Console.WriteLine(JsonConvert.SerializeObject(availabilityRq, Formatting.Indented, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore }));
                AvailabilityRS responseAvail = client.doAvailability(availabilityRq);

            var response = new SearchHotelResult();
            List<HotelDetail> hotels = new List<HotelDetail>();
            
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
                        Latitude = decimal.Parse(hotelResponse.latitude),
                        Longitude = decimal.Parse(hotelResponse.longitude),
                        ZoneCode =  hotelResponse.zoneCode,
                        DestinationCode = hotelResponse.destinationCode,
                        NetFare =  hotelResponse.totalNet,
                        StarRating =  hotelResponse.categoryCode,
                        OriginalFare = hotelResponse.minRate,
                        Review = hotelResponse.reviews,
                        Rooms =hotelResponse.rooms.Select(roomApi=>new HotelRoom
                        {
                            RoomCode = roomApi.code,
                            Rates = roomApi.rates.Select(x=> new HotelRate
                            {
                                RateKey = x.rateKey,
                                Price = new Price{OriginalIdr = x.net},
                                Boards = x.boardCode,
                                //Cancellation = x.cancellationPolicies.Select(y=> new Cancellation
                                //{
                                //    Fee = y.amount,
                                //    StartTime = y.from
                                //}).ToList(),
                                Class = x.rateClass,
                                Type = x.rateType.ToString() 
                            }).ToList()
                        }).ToList(),
                    };
                    hotels.Add(hotel);
                }
                response.HotelDetails = hotels;
            }
            return response;
        }
    }
}
