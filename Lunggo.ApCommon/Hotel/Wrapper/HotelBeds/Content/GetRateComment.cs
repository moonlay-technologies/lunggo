using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content
{
    public class GetRateComment
    {
        public void GetRateCommentData()
        {
            try
            {
                var client = new HotelApiClient("p8zy585gmgtkjvvecb982azn", "QrwuWTNf8a",
                    "https://api.test.hotelbeds.com/hotel-content-api");

                var dataCount = 1;
                var counter = 0;
                var from = 1;
                var to = 1000;
                while (to <= 108000)
                {

                    Debug.Print("From : " + from);
                    Debug.Print("To : " + to);

                    Console.WriteLine("RateFrom : " + from);

                    List<Tuple<string, string>> param;

                    //Call for the first time 
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
                        //var rateComment = new HotelRateComment
                        //{
                        //    Code = rateComs.code,
                        //    HotelCode = rateComs.hotel,
                        //    Incoming = rateComs.incoming,
                        //    CommentsByRates = rateComs.commentsByRates == null ? null : rateComs.commentsByRates.Select(x => new CommentsByRates
                        //    {
                        //        RateCodes = x.rateCodes,
                        //        Comments = x.comments == null ? null : x.comments.Select(p => new Comment
                        //        {
                        //            DateStart = p.dateStart,
                        //            DateEnd = p.dateEnd,
                        //            Description = p.description
                        //        }).ToList(),
                        //    }).ToList()
                        //};
                       
                    }
                    from = from + 1000;
                    to = to + 1000;
                    Thread.Sleep(1000);

                }
                Console.WriteLine("Done");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        public void InsertRateCommentToTableStorage(HotelRateComment rateComments)
        {
            HotelService.GetInstance().SaveRateCommentToTableStorage(rateComments);
        }
    }
}
