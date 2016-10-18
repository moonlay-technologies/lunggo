using System.Diagnostics;
using System.Threading;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.messages;
using Lunggo.Framework.Extension;
using Lunggo.Framework.TicketSupport.ZendeskClass;
using Microsoft.Data.Edm.Csdl;
using Lunggo.Framework.Documents;
using Image = Lunggo.ApCommon.Hotel.Model.Image;
using Terminal = Lunggo.ApCommon.Hotel.Model.Terminal;

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

            var dataCount = 1; 
            var counter = 0;
            var from = 1;
            var to = 1000;
            while (to <= 128000)
            {

                Debug.Print("From : " + from);
                Debug.Print("To : " + to);

                Console.WriteLine("From : " + from);

                foreach (var t in languageCd)
                {
                    Debug.Print("BAHASA : " + t);
                    List<Tuple<string, string>> param;

                    //Call for the first time 
                    param = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("${fields}", "all"),
                    new Tuple<string, string>("${language}", t),
                    new Tuple<string, string>("${from}", from.ToString()),
                    new Tuple<string, string>("${to}", to.ToString()), //1000
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
                                Terminals = hotelRS.terminals == null?null:hotelRS.terminals.Select(p => new Terminal()
                                {
                                       Name = p.name,
                                       TerminalCode = p.terminalCode,
                                       Distance = p.distance,
                                       Description = p.description
                                }).ToList(),
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
                                        Type = p.roomType,
                                        characteristicCd = p.characteristicCode,
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
                                ImageUrl = hotelRS.images == null ? null : hotelRS.images.Select(p => new Image
                                {
                                    Order = p.order,
                                    Path = p.path,
                                    Type = p.imageTypeCode,
                                }).ToList(),

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
                                languageCode = "IND",
                                Description = hotelRS.description == null ? null : hotelRS.description.content
                            });

                            Debug.Print("Insert ke-" + dataCount + " : " + hotels[counter].HotelCode);
                            InsertHotelDetailToTableStorage(hotels[counter]);
                            counter++;
                            dataCount++;
                        }

                    }
                }
                from = from + 1000;
                to = to + 1000;
                Thread.Sleep(1000);

            }
            Console.WriteLine("Done");
        }

        public void InsertHotelDetailToTableStorage(HotelDetailsBase hotel)
        {
            HotelService.GetInstance().SaveHotelDetailToTableStorage(hotel,hotel.HotelCode);
            
        }

    }
}
