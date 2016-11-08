using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.helpers;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.Framework.Context;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json;
using Lunggo.ApCommon.Hotel.Constant;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds
{
    public class HotelBedsSearchHotel
    {
        public SearchHotelResult SearchHotel(SearchHotelCondition condition)
        {
            HotelApiClient client = new HotelApiClient("p8zy585gmgtkjvvecb982azn", "QrwuWTNf8a", "https://api.test.hotelbeds.com/hotel-api");
            var avail = new Availability();
            if (condition.Destination != null || condition.Zone != null || condition.Area != null)
            {
                var location = condition.Destination ?? condition.Zone ?? condition.Area;
                var includedHotels = HotelService.GetInstance().GetHotelListByLocationFromStorage(location);
                avail = new Availability
                {
                checkIn = condition.CheckIn,
                    checkOut = condition.Checkout,
                    //destination = condition.Destination,
                    //zone = condition.Zone != null ? int.Parse(condition.Zone.Split('-')[1]) : (int?) null,
                language = "ENG",
                    payed = Availability.Pay.AT_WEB,
                    includeHotels = includedHotels
            };
            }
            else
            {
                avail = new Availability
                {
                    checkIn = condition.CheckIn,
                    checkOut = condition.Checkout,
                    includeHotels = new List<int> { condition.HotelCode },
                    language = "ENG",
                    payed = Availability.Pay.AT_WEB
                };
            }

            if (condition.Occupancies == null)
            {
                AvailRoom room = new AvailRoom
                {
                    adults = condition.AdultCount,
                    children = condition.ChildCount,
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
            }
            else
            {
                foreach (var occ in condition.Occupancies)
                {
                    AvailRoom room = new AvailRoom
                    {
                        adults = occ.AdultCount,
                        children = occ.ChildCount,
                        numberOfRooms = occ.RoomCount
                    };
                    room.details = new List<RoomDetail>();
                    for (int i = 0; i < occ.AdultCount; i++)
                    {
                        room.adultOf(30);
                    }

                    if (occ.ChildrenAges != null)
                    {
                        //occ.ChildrenAges.Sort();
                        occ.ChildrenAges = occ.ChildrenAges.Take(occ.ChildCount).ToList();
                    }
                    
                    for (int i = 0; i < occ.ChildCount; i++)
                    {
                        room.childOf(occ.ChildrenAges[i]);
                    }
                    avail.rooms.Add(room);
                }
            }
            
            AvailabilityRQ availabilityRq = avail.toAvailabilityRQ();
                if (availabilityRq == null)
                    throw new Exception("Availability RQ can't be null", new ArgumentNullException());

                Console.WriteLine(JsonConvert.SerializeObject(availabilityRq, Formatting.Indented, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore }));
                AvailabilityRS responseAvail = client.doAvailability(availabilityRq);

            var response = new SearchHotelResult();
            List<HotelDetail> hotels = new List<HotelDetail>();
            
            //var hotels = new HotelDetail();
            var lang = OnlineContext.GetActiveLanguageCode();
            var allCurrencies = HotelService.GetInstance().GetAllCurrenciesFromCache(condition.SearchId);
            if (responseAvail != null && responseAvail.hotels != null && responseAvail.hotels.hotels != null &&
                responseAvail.hotels.hotels.Count > 0)
            {
                foreach (var hotelResponse in responseAvail.hotels.hotels)
                {
                    var hotel = new HotelDetail()
                    {
                        HotelCode = hotelResponse.code,
                        HotelName = hotelResponse.name,
                        CountryCode = HotelService.GetInstance().GetCountryFromDestination(hotelResponse.destinationCode),
                        Latitude = hotelResponse.latitude == null ? null : (decimal?)decimal.Parse(hotelResponse.latitude),
                        Longitude = hotelResponse.longitude == null ? null : (decimal?)decimal.Parse(hotelResponse.longitude),
                        ZoneCode = hotelResponse.destinationCode + '-' + hotelResponse.zoneCode.ToString(CultureInfo.InvariantCulture),
                        DestinationCode = hotelResponse.destinationCode,
                        StarRating =  hotelResponse.categoryCode,
                        Review = hotelResponse.reviews,
                        Rooms = hotelResponse.rooms == null ? null : hotelResponse.rooms.Select(roomApi => new HotelRoom
                        {
                            RoomCode = roomApi.code,
                            Type = roomApi.code.Substring(0, 3),
                            TypeName = lang == "EN" ? HotelService.GetInstance().GetHotelRoomTypeDescEn(roomApi.code.Substring(0, 3)) :
                            HotelService.GetInstance().GetHotelRoomTypeDescId(roomApi.code.Substring(0, 3)),
                            RoomName = roomApi.name,
                            Rates = roomApi.rates == null ? null : roomApi.rates.Select(x =>
                            {
                                var rate = new HotelRate
                                {
                                    AdultCount = x.adults,
                                    ChildCount = x.children,
                                    RoomCount = x.rooms,
                                    Allotment = x.allotment,
                                    PaymentType = PaymentTypeCd.Mnemonic(x.paymentType),
                                    Offers = x.offers == null ? null : x.offers.Select(z => new Offer
                                    {
                                        Code = z.code,
                                        Amount = z.amount,
                                        Name = z.name
                                    }).ToList(),
                                RateKey = x.rateKey,
                                    Price = new Price(),
                                Boards = x.boardCode,
                                    Cancellation = x.cancellationPolicies == null ? null : x.cancellationPolicies.Select(y => new Cancellation
                                    {
                                        Fee = y.amount,
                                        StartTime = y.from
                                    }).ToList(),
                                Class = x.rateClass,
                                Type = x.rateType.ToString(),
                                TimeLimit = DateTime.UtcNow.AddMinutes(30),
                                HotelSellingRate = x.hotelMandatory?x.hotelSellingRate:0M,
                                RateCommentsId = x.rateCommentsId
                                };
                                rate.Price.SetSupplier(x.net,
                                    x.hotelCurrency != null ? allCurrencies[x.hotelCurrency] : allCurrencies["IDR"]);
                                return rate;
                            }).ToList()
                        }).ToList(),
                    };
                    DistinguishRoom(hotel); 
                    hotels.Add(hotel);
                }
                response.HotelDetails = hotels;
                response.CheckIn = condition.CheckIn;
                response.CheckOut = condition.Checkout;
            }
            return response;
        }

        private void DistinguishRoom(HotelDetail hotel)
        {
            var newlist = new List<HotelRoom>();
            foreach (var room in hotel.Rooms)
            {
                var found = newlist.SingleOrDefault(r => r.RoomCode == room.RoomCode);
                if (found != null)
                {
                    found.Rates.AddRange(room.Rates);
                }
                else
                {
                    newlist.Add(room);
                }
            }

            hotel.Rooms = newlist;
        }
    }
}
