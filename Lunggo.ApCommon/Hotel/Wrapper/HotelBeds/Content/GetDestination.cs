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
        public static List<DestinationApi> hotelDestinationList = new List<DestinationApi>();
        public void GetDestination(int from, int to)
        {
            try
            {
                //var client = new HotelApiClient("zvwtnf83dj86bf58sejb6e3f", "HBbpT4u3xE",
                //    "https://api.test.hotelbeds.com/hotel-content-api");
                var client = new HotelApiClient(HotelApiType.ContentApi);
                var total = GetTotalDestination(client);
                Console.WriteLine("Total Destination : " + total);
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
                    DoGetDestination(client, from, to);

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
                GetDestination(from, to);
            }
        }

        public void DoGetDestination(HotelApiClient client, int from, int to)
        {
            List<Tuple<string, string>> param;

            param = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("${fields}", "code%2C+name%2CcountryCode%2CisoCode%2Czones"),
                new Tuple<string, string>("${language}", "ENG"),
                new Tuple<string, string>("${from}", from.ToString()),
                new Tuple<string, string>("${to}", to.ToString()),
                new Tuple<string, string>("${useSecondaryLanguage}", "false"),
            };
            var destinationRs = client.GetDestination(param);
            if (destinationRs != null && destinationRs.destinations.Count != 0)
            {
                if (hotelDestinationList == null && hotelDestinationList.Count == 0)
                {
                    hotelDestinationList = destinationRs.destinations;
                }
                else
                {
                    hotelDestinationList.AddRange(destinationRs.destinations);
                }
            }
        }

        public int GetTotalDestination(HotelApiClient client)
        {
            List<Tuple<string, string>> param;

            param = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("${fields}", "code%2C+name%2CcountryCode%2CisoCode%2Czones"),
                new Tuple<string, string>("${language}", "ENG"),
                new Tuple<string, string>("${from}", "1"),
                new Tuple<string, string>("${to}", "10"),
                new Tuple<string, string>("${useSecondaryLanguage}", "false"),
            };
            var DestinationRs = client.GetDestination(param);
            var total = DestinationRs.total;
            return total;
        }
    }
}
