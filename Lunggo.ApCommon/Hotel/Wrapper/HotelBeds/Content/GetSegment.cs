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
        public static List<SegmentApi> hotelSegmentList = new List<SegmentApi>();
        public void GetSegment(int from, int to)
        {
            try
            {
                //var client = new HotelApiClient("zvwtnf83dj86bf58sejb6e3f", "HBbpT4u3xE",
                //    "https://api.test.hotelbeds.com/hotel-content-api");
                var client = new HotelApiClient(HotelApiType.ContentApi);
                var total = GetTotalSegment(client);
                Console.WriteLine("Total Segment : " + total);
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
                    DoGetSegment(client, from, to);

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
                GetSegment(from, to);
            }
        }

        public void DoGetSegment(HotelApiClient client, int from, int to)
        {
            var languageCd = new List<string> { "ENG", "IND" };
            var segmentTemp = new List<SegmentApi>();
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
                var segmentRs = client.GetSegment(param);
                if (segmentRs != null && segmentRs.segments != null && segmentRs.segments.Count != 0)
                {
                    foreach (var segment in segmentRs.segments)
                    {
                        if (t.Equals("ENG"))
                        {
                            var singlesegment = new SegmentApi
                            {
                                code = segment.code,
                                DescriptionEng = segment.description == null ? null : segment.description.content
                            };
                            segmentTemp.Add(singlesegment);
                        }
                        else
                        {
                            segmentTemp[counter].DescriptionInd = segment.description == null
                                ? null
                                : segment.description.content;
                            counter++;
                        }
                    }
                }
            }
            hotelSegmentList.AddRange(segmentTemp);
            
        }

        public int GetTotalSegment(HotelApiClient client)
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
            var SegmentRs = client.GetSegment(param);
            var total = SegmentRs.total;
            return total;
        }
    }
}
