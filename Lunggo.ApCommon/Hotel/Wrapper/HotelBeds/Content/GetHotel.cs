using System.Diagnostics;
using System.Threading;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages;
using Microsoft.Data.Edm.Csdl;
using Lunggo.Framework.Documents;

namespace Lunggo.ApCommon.Hotel.Wrapper.Content
{
    public class GetHotel
    {
        public void GetHotelData()
        {

            var hotels = new List<HotelDetailsBase>();
            var languageCd = new List<string>{"ENG","IND"};
            
            var client = new HotelApiClient("p8zy585gmgtkjvvecb982azn", "QrwuWTNf8a", "https://api.test.hotelbeds.com/hotel-content-api");
           //https://api.test.hotelbeds.com/hotel-content-api/1.0/hotels?fields=all&language=ENG&from=1&to=100&useSecondaryLanguage=false

            var counter = 0;
            var max = 100;
            var start = 1;
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            foreach (var t in languageCd)
            {
                Debug.Print("BAHASA : " + t);
                List<Tuple<string, string>> param;

                //Call for the first time 
                param = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("${fields}", "all"),
                    new Tuple<string, string>("${language}", t),
                    new Tuple<string, string>("${from}", "1"),
                    new Tuple<string, string>("${to}", "1000"), //1000
                    new Tuple<string, string>("${useSecondaryLanguage}", "false"),
                };
                var bookingListRS = client.GetHotelList(param);
                var hotelDescription = new List<HotelDescriptions>();
                foreach (var hotelRS in bookingListRS.hotels)
                {
                    if (t.Equals("ENG"))
                    {
                        var hotel = new HotelDetailsBase
                        {
                            HotelCode = hotelRS.code,
                            HotelName = hotelRS.name == null ? null : hotelRS.name.content,
                            AccomodationType = hotelRS.accommodationTypeCode,
                            Address = hotelRS.address == null ? null : hotelRS.address.content,
                            Chain = hotelRS.chainCode,
                            Email = hotelRS.email,
                            PhonesNumbers =
                                hotelRS.phones == null ? null : hotelRS.phones.Select(p => p.phoneNumber).ToList(),
                            City = hotelRS.city == null ? null : hotelRS.city.content,
                            PostalCode = hotelRS.postalCode,
                            StarRating = hotelRS.categoryCode,
                            Terminals =
                                hotelRS.terminals == null
                                    ? null
                                    : hotelRS.terminals.Select(p => p.terminalCode).ToList(),
                            ZoneCode = hotelRS.zoneCode,
                            CountryCode = hotelRS.countryCode,
                            Pois = hotelRS.InterestPoints == null
                                ? null
                                : hotelRS.InterestPoints.Select(p => new POI()
                                {
                                    poiName = p.poiName,
                                    distance = p.distance,
                                    facilityCode = p.facilityCode,
                                    facilityGroupCode = p.facilityGroupCode,
                                    order = p.order
                                }).ToList(),
                            Latitude = hotelRS.coordinates == null ? 0 : hotelRS.coordinates.latitude,
                            Longitude = hotelRS.coordinates == null ? 0 : hotelRS.coordinates.longitude,
                            Segment = hotelRS.segmentCodes,
                            Rooms = hotelRS.rooms == null
                                ? null
                                : hotelRS.rooms.Select(p => new HotelRoom
                                {
                                    RoomCode = p.roomCode,
                                    Facilities = p.roomFacilities == null ? null : p.roomFacilities.Select(q => new HotelRoomFacilities
                                    {
                                        FacilityCode = q.facilityCode,
                                        FacilityGroupCode = q.facilityGroupCode
                                    }).ToList()
                                }).ToList(),
                            Facilities = hotelRS.facilities == null ? null : hotelRS.facilities.Select(p => new HotelFacility()
                            {
                                FacilityCode = p.facilityCode,
                                FacilityGroupCode = p.facilityGroupCode
                            }).ToList(),
                            DestinationCode = hotelRS.destinationCode,
                            ImageUrl = hotelRS.images == null ? null : hotelRS.images.Select(p => p.path).ToList(),
                            
                        };
                        var hotelDesc = new HotelDescriptions
                        {
                            languageCode = "ENG",
                            Description = hotelRS.description == null ? null : hotelRS.description.content
                        };
                        hotel.Description = new List<HotelDescriptions>();
                        hotel.Description.Add(hotelDesc);
                        hotels.Add(hotel);
                    }
                    else
                    {
                        hotels[counter].Description.Add(new HotelDescriptions
                        {
                            languageCode = "ID",
                            Description = hotelRS.description == null ? null : hotelRS.description.content
                        });
                        if (start != max)
                        {
                            Debug.Print("Inserting : " + hotels[counter].HotelCode);
                            InsertHotelDetailToDocDB(hotels[counter]);
                            start++;
                            counter++;
                        }
                        else
                        {
                            start = 1;
                            Thread.Sleep(2000);
                        }
                    }
                    
                }


                var total = bookingListRS.total; //bookingListRS.total
                var from = 1001; //1001
                var to = 2000; //2000
                var isValid = true;
                do
                {
                    if (to >= total)
                    {
                        to = total;
                        isValid = false;
                    }
                    param = new List<Tuple<string, string>>
                    {
                        new Tuple<string, string>("${fields}", "all"),
                        new Tuple<string, string>("${language}", t),
                        new Tuple<string, string>("${from}", @from.ToString()),
                        new Tuple<string, string>("${to}", to.ToString()),
                        new Tuple<string, string>("${useSecondaryLanguage}", "false"),
                    };
                    Debug.Print("From : " + @from);
                    Debug.Print("To : " + to);
                    bookingListRS = client.GetHotelList(param);
                    foreach (var hotelRS in bookingListRS.hotels)
                    {
                        if (t.Equals("ENG"))
                        {
                            var hotel = new HotelDetailsBase
                            {
                                HotelCode = hotelRS.code,
                                HotelName = hotelRS.name == null ? null : hotelRS.name.content,
                                AccomodationType = hotelRS.accommodationTypeCode,
                                Address = hotelRS.address == null ? null : hotelRS.address.content,
                                Chain = hotelRS.chainCode,
                                Email = hotelRS.email,
                                PhonesNumbers = hotelRS.phones == null ? null : hotelRS.phones.Select(p => p.phoneNumber).ToList(),
                                City = hotelRS.city == null ? null : hotelRS.city.content,
                                PostalCode = hotelRS.postalCode,
                                StarRating = hotelRS.categoryCode,
                                Terminals = hotelRS.terminals == null ? null : hotelRS.terminals.Select(p => p.terminalCode).ToList(),
                                ZoneCode = hotelRS.zoneCode,
                                CountryCode = hotelRS.countryCode,
                                Pois = hotelRS.InterestPoints == null ? null : hotelRS.InterestPoints.Select(p => new POI()
                                {
                                    poiName = p.poiName,
                                    distance = p.distance,
                                    facilityCode = p.facilityCode,
                                    facilityGroupCode = p.facilityGroupCode,
                                    order = p.order
                                }).ToList(),
                                Latitude = hotelRS.coordinates == null ? 0 : hotelRS.coordinates.latitude,
                                Longitude = hotelRS.coordinates == null ? 0 : hotelRS.coordinates.longitude,
                                Segment = hotelRS.segmentCodes,
                                Rooms = hotelRS.rooms == null ? null : hotelRS.rooms.Select(p => new HotelRoom
                                {
                                    RoomCode = p.roomCode
                                }).ToList(),
                                Facilities = hotelRS.facilities == null ? null : hotelRS.facilities.Select(p => new HotelFacility()
                                {
                                    FacilityCode = p.facilityCode,
                                    FacilityGroupCode = p.facilityGroupCode
                                }).ToList(),
                                DestinationCode = hotelRS.destinationCode,
                                ImageUrl = hotelRS.images == null ? null : hotelRS.images.Select(p => p.path).ToList(),
                            };
                            var hotelDesc = new HotelDescriptions
                            {
                                languageCode = "ENG",
                                Description = hotelRS.description == null ? null : hotelRS.description.content
                            };
                            hotel.Description = new List<HotelDescriptions>();
                            hotel.Description.Add(hotelDesc);
                            hotels.Add(hotel);
                        }
                        else
                        {
                            hotels[counter].Description.Add(new HotelDescriptions
                            {
                                languageCode = "ID",
                                Description = hotelRS.description == null ? null : hotelRS.description.content
                            });
                            if (start != max)
                            {
                                Debug.Print("Inserting : " + hotels[counter]);
                                InsertHotelDetailToDocDB(hotels[counter]);
                                start++;
                                counter++;
                            }
                            else
                            {
                                start = 1;
                                Thread.Sleep(2000);
                            }
                        }
                    }

                    @from = @from + 1000; //1000
                    to = to + 1000; //1000
                } while (isValid);
            }
            stopwatch.Stop();
            Debug.Print("Time elapsed: {0}", stopwatch.Elapsed);
            Console.WriteLine("Done");
        }

        public void InsertHotelDetailToDocDB(HotelDetailsBase hotel)
        {
            var document = DocumentService.GetInstance();
                document.Upsert("HotelDetail:" + hotel.HotelCode, hotel);
        }

    }
}
