using System.Diagnostics;
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

            List<HotelDetailsBase> hotels = new List<HotelDetailsBase>();
            List<string> languageCd = new List<string>{"ENG","IND"};
            
            HotelApiClient client = new HotelApiClient("p8zy585gmgtkjvvecb982azn", "QrwuWTNf8a", "https://api.test.hotelbeds.com/hotel-content-api");
           //https://api.test.hotelbeds.com/hotel-content-api/1.0/hotels?fields=all&language=ENG&from=1&to=100&useSecondaryLanguage=false

            int counter = 0;
            for (int i = 0; i < languageCd.Count; i++)
            {
                Debug.Print("BAHASA : " + languageCd[i]);
                List<Tuple<string, string>> param;

                //Call for the first time 
                param = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("${fields}", "all"),
                    new Tuple<string, string>("${language}", languageCd[i]),
                    new Tuple<string, string>("${from}", "1"),
                    new Tuple<string, string>("${to}", "3"), //1000
                    new Tuple<string, string>("${useSecondaryLanguage}", "false"),
                };
                HotelRS bookingListRS = client.GetHotelList(param);
                List<HotelDescriptions> hotelDescription = new List<HotelDescriptions>();
                foreach (var hotelRS in bookingListRS.hotels)
                {
                    if (languageCd[i].Equals("ENG"))
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
                                    RoomCode = p.roomCode
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
                        counter++;
                    }
                    
                }


                int total = 11; //bookingListRS.total
                int from = 4; //1001
                int to = 6; //2000
                bool isValid = true;
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
                    new Tuple<string, string>("${language}", languageCd[i]),
                    new Tuple<string, string>("${from}", from.ToString()),
                    new Tuple<string, string>("${to}", to.ToString()),
                    new Tuple<string, string>("${useSecondaryLanguage}", "false"),
                };
                    Debug.Print("From : " + from);
                    Debug.Print("To : " + to);
                    bookingListRS = client.GetHotelList(param);
                    foreach (var hotelRS in bookingListRS.hotels)
                    {
                        if (languageCd[i].Equals("ENG"))
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
                            counter++;
                        }
                        
                    }

                    from = from + 3; //1000
                    to = to + 3; //1000
                } while (isValid);  
            }

            Console.WriteLine("Done");
            
           var document = DocumentService.GetInstance();
           document.Upsert("HotelDetails",hotels);

        }

    }
}
