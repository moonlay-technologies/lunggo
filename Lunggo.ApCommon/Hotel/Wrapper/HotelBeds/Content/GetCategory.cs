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
        public static List<CategoryApi> hotelCategoryList = new List<CategoryApi>();
        public void GetCategory(int from, int to)
        {
            try
            {
                //var client = new HotelApiClient("zvwtnf83dj86bf58sejb6e3f", "HBbpT4u3xE",
                //    "https://api.test.hotelbeds.com/hotel-content-api");
                var client = new HotelApiClient(HotelApiType.ContentApi);
                var total = GetTotalCategory(client);
                Console.WriteLine("Total Category : " + total);
                var isValid = true;
                do
                {
                    Debug.Print("From : " + from);

                    Console.WriteLine("RateFrom : " + from);

                    if (to >= total)
                    {
                        to = total;
                        isValid = false;
                    }
                    Debug.Print("To : " + to);
                    DoGetCategory(client, from, to);

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
                GetCategory(from, to);
            }
        }

        public void DoGetCategory(HotelApiClient client, int from, int to)
        {
            var languageCd = new List<string> { "ENG", "IND" };
            var categoryTemp = new List<CategoryApi>();
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
                var categoryRs = client.GetCategory(param);
                if (categoryRs != null && categoryRs.categories != null && categoryRs.categories.Count != 0)
                {
                    foreach (var category in categoryRs.categories)
                    {
                        if (t.Equals("ENG"))
                        {
                            var singlecategory = new CategoryApi
                            {
                                code = category.code,
                                simpleCode = category.simpleCode,
                                accomodationType = category.accomodationType,
                                DescriptionEng = category.description == null ? null : category.description.content
                            };
                            categoryTemp.Add(singlecategory);
                        }
                        else
                        {
                            categoryTemp[counter].DescriptionInd = category.description == null
                                ? null
                                : category.description.content;
                            counter++;
                        }
                    }
                }
            }
            hotelCategoryList.AddRange(categoryTemp);
            
        }

        public int GetTotalCategory(HotelApiClient client)
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
            var categoryRs = client.GetCategory(param);
            var total = categoryRs.total;
            return total;
        }
    }
}
