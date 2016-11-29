using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content
{
    public class GetRateComment
    {
        public void GetRateCommentData(int from, int to)
        {
            try
            {
                //var client = new HotelApiClient("zvwtnf83dj86bf58sejb6e3f", "HBbpT4u3xE",
                //    "https://api.test.hotelbeds.com/hotel-content-api");
                var client = new HotelApiClient(HotelApiType.ContentApi);
                var total = GetTotalRateComment(client);
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
                    DoGetRateComment(client, from, to);

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
                GetRateCommentData(from, to);
            }
        }

        public void DoGetRateComment(HotelApiClient client, int from, int to)
        {
            var dataCount = from;
            var counter = 0;
            List<Tuple<string, string>> param;

            param = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("${fields}", "all"),
                new Tuple<string, string>("${language}", "ENG"),
                new Tuple<string, string>("${from}", from.ToString()),
                new Tuple<string, string>("${to}", to.ToString()),
                new Tuple<string, string>("${useSecondaryLanguage}", "false"),
            };
            var rateCommentRs = client.GetRateComment(param);
            foreach (var rateComs in rateCommentRs.rateComments)
            {
                foreach (var rates in rateComs.commentsByRates)
                {
                    foreach (var rateCd in rates.rateCodes)
                    {
                        foreach (var date in rates.comments)
                        {
                            var rateComment = new HotelRateComment
                            {
                                Code = rateComs.code,
                                Incoming = rateComs.incoming,
                                HotelCode = rateComs.hotel,
                                RateCode = rateCd,
                                DateStart = date.dateStart,
                                DateEnd = date.dateEnd,
                                Description = date.description
                            };
                            Debug.Print("Insert ke-" + dataCount);
                            InsertRateCommentToTableStorage(rateComment);
                            dataCount++;
                        }
                    }
                }
            }
        }

        public int GetTotalRateComment(HotelApiClient client)
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
            var rateCommentRs = client.GetRateComment(param);
            var total = rateCommentRs.total;
            return total;
        }

        public void InsertRateCommentToTableStorage(HotelRateComment rateComments)
        {
            HotelService.GetInstance().SaveRateCommentToTableStorage(rateComments);
        }
    }
}
