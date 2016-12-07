using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content
{
    public partial class HotelBedsService
    {
        public void GetFacilityGroup(int from, int to)
        {
            try
            {
                //var client = new HotelApiClient("zvwtnf83dj86bf58sejb6e3f", "HBbpT4u3xE",
                //    "https://api.test.hotelbeds.com/hotel-content-api");
                var client = new HotelApiClient(HotelApiType.ContentApi);
                var total = GetTotalFacilityGroup(client);
                Console.WriteLine("Total Facility Group : " + total);
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
                    DoGetFacilityGroup(client, from, to);

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
                GetFacilityGroup(from, to);
            }
        }

        public void DoGetFacilityGroup(HotelApiClient client, int from, int to)
        {
            var languageCd = new List<string> { "ENG", "IND" };

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
                var FacilityGroupRs = client.GetFacilityGroup(param);
            }

        }

        public int GetTotalFacilityGroup(HotelApiClient client)
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
            var FacilityGroupRs = client.GetFacilityGroup(param);
            var total = FacilityGroupRs.total;
            return total;
        }
    }
}
