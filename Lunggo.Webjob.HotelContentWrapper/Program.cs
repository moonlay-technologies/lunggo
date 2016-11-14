using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.Content;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Documents;
using Lunggo.Framework.TableStorage;

namespace Lunggo.Webjob.HotelContentWrapper
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Init();
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            //HotelService.GetInstance().SaveTruncatedHotelDetail();
            //HotelService.GetInstance().UpdateHotelAmenitiesContent();

            var hotel = new GetHotel();
            var service = HotelService.GetInstance();
            var rate = new GetRateComment();
            Console.WriteLine("Getting Hotel Detail");
            hotel.GetHotelData(1,128000);

            Console.WriteLine("Update Hotel List by Location Content");
            service.UpdateHotelListByLocationContent();

            Console.WriteLine("Update Hotel Detail by Location Content");
            //rate.GetRateCommentData(); //TODO

            Console.WriteLine("RateComment");
            rate.GetRateCommentData();
            
            stopwatch.Stop();
            Debug.Print("Done in : {0}", stopwatch.Elapsed);
            Console.WriteLine("Done in : {0}", stopwatch.Elapsed);
        }
    }
}
