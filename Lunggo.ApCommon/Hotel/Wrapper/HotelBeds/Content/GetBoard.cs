using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Wrapper.AirAsia;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content
{
    public partial class HotelBedsService
    {
        public static List<BoardApi> hotelBoardList = new List<BoardApi>();
        public void GetBoard(int from, int to)
        {
            try
            {
                //var client = new HotelApiClient("zvwtnf83dj86bf58sejb6e3f", "HBbpT4u3xE",
                //    "https://api.test.hotelbeds.com/hotel-content-api");
                var client = new HotelApiClient(HotelApiType.ContentApi);
                var total = GetTotalBoard(client);
                Console.WriteLine("Total Board : " + total);
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
                    DoGetBoard(client, from, to);

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
                GetBoard(from, to);
            }
        }

        public void DoGetBoard(HotelApiClient client, int from, int to)
        {
            var languageCd = new List<string> { "ENG", "IND" };
            var boardTemp = new List<BoardApi>();
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
                var boardRs = client.GetBoard(param);
                if (boardRs != null && boardRs.boards != null && boardRs.boards.Count != 0)
                {
                    foreach (var board in boardRs.boards)
                    {
                        if (t.Equals("ENG"))
                        {
                            var singleBoard = new BoardApi
                            {
                                code = board.code,
                                multiLingualCode = board.multiLingualCode,
                                DescriptionEng = board.description == null ? null : board.description.content
                            };
                            boardTemp.Add(singleBoard);
                        }
                        else
                        {
                            boardTemp[counter].DescriptionInd = board.description == null
                                ? null
                                : board.description.content;
                            counter++;
                        }
                    }
                }
            }
            hotelBoardList.AddRange(boardTemp);
        }

        public int GetTotalBoard(HotelApiClient client)
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
            var boardRs = client.GetBoard(param);
            var total = boardRs.total;
            return total;
        }
    }
}
