using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content
{
    public partial class HotelBedsService
    {
        public void GetHotelData(int from, int to)
        {

            try
            {
                //var client = new HotelApiClient("zvwtnf83dj86bf58sejb6e3f", "HBbpT4u3xE",
                //    "https://api.test.hotelbeds.com/hotel-content-api");
                var client = new HotelApiClient(HotelApiType.ContentApi);
                //StatusRS status = client.status();

                //if (status != null && status.error == null)
                //    Debug.Print("StatusRS: " + status.status);
                //else if (status != null && status.error != null)
                //{
                //    Debug.Print("StatusRS: " + status.status + " " + status.error.code + ": " + status.error.message);
                //    return;
                //}
                //else if (status == null)
                //{
                //    Debug.Print("StatusRS: Is not available.");
                //    return;
                //}
                var total = GetTotalHotel(client);
                var isValid = true;
                do
                {
                    Debug.Print("From : " + from);
                    Debug.Print("To : " + to);
                    Console.WriteLine("HotelFrom : " + from);

                    if (to >= total)
                    {
                        to = total;
                        isValid = false;
                    }

                    DoGetHotelDetail(client, from, to);

                    from = from + 1000;
                    to = to + 1000;
                    Thread.Sleep(1000);

                } while (isValid);
            }
            catch
            {
                Console.WriteLine(from);
                Console.WriteLine(to);
                GetHotelData(from, to);
            }
        }

        public void DoGetHotelDetail(HotelApiClient client, int from, int to)
        {
            var hotels = new List<HotelDetailsBase>();
            var languageCd = new List<string> { "ENG", "IND" };
            var dataCount = from;
            var counter = 0;
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
                            new Tuple<string, string>("${to}", to.ToString()),
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
                                hotelRS.phones == null
                                    ? null
                                    : hotelRS.phones.Select(p => p.phoneNumber).ToList(),
                            City = hotelRS.city == null ? null : hotelRS.city.content,
                            PostalCode = hotelRS.postalCode ?? null,
                            StarRating = hotelRS.categoryCode,
                            Terminals =
                                hotelRS.terminals == null
                                    ? null
                                    : hotelRS.terminals.Select(p => new Terminal()
                                    {
                                        Name = p.name,
                                        TerminalCode = p.terminalCode,
                                        Distance = p.distance,
                                        Description = p.description
                                    }).ToList(),
                            ZoneCode = hotelRS.destinationCode + '-' + hotelRS.zoneCode.ToString(CultureInfo.InvariantCulture),
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
                                    Facilities =
                                        p.roomFacilities == null
                                            ? null
                                            : p.roomFacilities.Select(q => new HotelRoomFacilities
                                            {
                                                FacilityCode = q.facilityCode,
                                                FacilityGroupCode = q.facilityGroupCode
                                            }).ToList()
                                }).ToList(),
                            Facilities =
                                hotelRS.facilities == null
                                    ? null
                                    : hotelRS.facilities.Select(p => new HotelFacility()
                                    {
                                        FacilityCode = p.facilityCode,
                                        FacilityGroupCode = p.facilityGroupCode,
                                        IsFree = !p.indFee,
                                        MustDisplay = p.indYesOrNo,
                                        IsAvailable = p.indLogic
                                    }).ToList(),
                            DestinationCode = hotelRS.destinationCode,
                            ImageUrl = hotelRS.images == null
                                ? null
                                : hotelRS.images.Select(p => new Image
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
        }

        public int GetTotalHotel(HotelApiClient client)
        {
            List<Tuple<string, string>> param;

            param = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("${fields}", "all"),
                new Tuple<string, string>("${language}", "ENG"),
                new Tuple<string, string>("${from}", "1"),
                new Tuple<string, string>("${to}", "10"),
                new Tuple<string, string>("${useSecondaryLanguage}", "false"),
            };
            var bookingListRS = client.GetHotelList(param);
            var total = bookingListRS.total;
            return total;
        }

        public void InsertHotelDetailToTableStorage(HotelDetailsBase hotel)
        {
            HotelService.GetInstance().SaveHotelDetailToTableStorage(hotel, hotel.HotelCode);
        }   
    }
}
