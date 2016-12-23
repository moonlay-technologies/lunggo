using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content
{
    public partial class HotelBedsService
    {
        public static List<RoomApi> hotelRoomList = new List<RoomApi>();
        public void GetRoom(int from, int to)
        {
            try
            {
                //var client = new HotelApiClient("zvwtnf83dj86bf58sejb6e3f", "HBbpT4u3xE",
                //    "https://api.test.hotelbeds.com/hotel-content-api");
                var client = new HotelApiClient(HotelApiType.ContentApi);
                var total = GetTotalRoom(client);
                Console.WriteLine("Total Room: " + total);
                var isValid = true;
                do
                {
                    Debug.Print("From : " + from);

                    Console.WriteLine("From : " + from);

                    if (to >= total)
                    {
                        to = total;
                        isValid = false;
                    }
                    Debug.Print("To : " + to);
                    DoGetRoom(client, from, to);

                    from = from + 1000;
                    to = to + 1000;
                    Thread.Sleep(1000);

                } while (isValid);
                Console.WriteLine("Done");
            }
            catch
            {
                Console.WriteLine(from);
                Console.WriteLine(to);
                GetRoom(from, to);
            }
        }

        public void DoGetRoom(HotelApiClient client, int from, int to)
        {
            var languageCd = new List<string> { "ENG", "IND" };
            var roomTemp = new List<RoomApi>();
            var counter = 0;
            foreach (var t in languageCd)
            {
                List<Tuple<string, string>> param;

                param = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("${fields}", "all"),
                new Tuple<string, string>("${language}", t),
                new Tuple<string, string>("${from}", from.ToString()),
                new Tuple<string, string>("${to}", to.ToString()),
                new Tuple<string, string>("${useSecondaryLanguage}", "false"),
            };
                var roomRs = client.GetRoom(param);
                if (roomRs != null && roomRs.rooms != null && roomRs.rooms.Count != 0)
                {
                    foreach (var room in roomRs.rooms)
                    {
                        if (t.Equals("ENG"))
                        {
                            var singleroom = new RoomApi
                            {
                                code = room.code,
                                characteristic = room.characteristic,
                                maxAdults = room.maxAdults,
                                minAdults = room.minAdults,
                                maxChildren = room.maxChildren,
                                maxPax = room.maxPax,
                                minPax = room.minPax,
                                type = room.type,
                                descriptionEng = room.description,
                                characteristicDescriptionEng = room.characteristicDescription == null ? null : room.characteristicDescription.content,
                                typeDescriptionInd = room.typeDescription == null ? null : room.typeDescription.content
                            };
                            roomTemp.Add(singleroom);
                        }
                        else
                        {
                            roomTemp[counter].characteristicDescriptionInd = room.characteristicDescription == null
                                ? null
                                : room.characteristicDescription.content;
                            roomTemp[counter].typeDescriptionInd = room.typeDescription == null ? null : room.typeDescription.content;
                            roomTemp[counter].descriptionInd = room.description;
                            counter++;
                        }
                    }
                }
            }
            hotelRoomList.AddRange(roomTemp);
        }

        public int GetTotalRoom(HotelApiClient client)
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
            var RoomRs = client.GetRoom(param);
            var total = RoomRs.total;
            return total;
        }
    }
}
