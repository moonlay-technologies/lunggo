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
        public static List<ChainApi> hotelChainList = new List<ChainApi>();
        public void GetChain(int from, int to)
        {
            try
            {
                //var client = new HotelApiClient("zvwtnf83dj86bf58sejb6e3f", "HBbpT4u3xE",
                //    "https://api.test.hotelbeds.com/hotel-content-api");
                var client = new HotelApiClient(HotelApiType.ContentApi);
                var total = GetTotalChain(client);
                Console.WriteLine("Total Chain: " + total);
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
                    DoGetChain(client, from, to);

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
                GetChain(from, to);
            }
        }

        public void DoGetChain(HotelApiClient client, int from, int to)
        {
            var languageCd = new List<string> { "ENG", "IND" };
            var chainTemp = new List<ChainApi>();
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
                var chainRs = client.GetChain(param);
                if (chainRs != null && chainRs.chains != null && chainRs.chains.Count != 0)
                {
                    foreach (var chain in chainRs.chains)
                    {
                        if (t.Equals("ENG"))
                        {
                            var singlechain = new ChainApi
                            {
                                code = chain.code,
                                DescriptionEng = chain.description == null ? null : chain.description.content
                            };
                            chainTemp.Add(singlechain);
                        }
                        else
                        {
                            chainTemp[counter].DescriptionInd = chain.description == null
                                ? null
                                : chain.description.content;
                            counter++;
                        }
                    }
                }
            }
            hotelChainList.AddRange(chainTemp);
            
        }

        public int GetTotalChain(HotelApiClient client)
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
            var chainRs = client.GetChain(param);
            var total = chainRs.total;
            return total;
        }
    }
}
