using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content;
using Lunggo.Framework.BlobStorage;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.Documents;
using Lunggo.Framework.TableStorage;
using RestSharp;

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

            var hotel = new HotelBedsService();
            var service = HotelService.GetInstance();
            /*Save Hotel Beds Content*/
            //hotel.GetAccomodation(1, 1000);
            //hotel.GetBoard(1, 1000);
            //hotel.GetCategory(1, 1000);
            //hotel.GetChain(1, 1000);
            //hotel.GetCountry(1, 1000);
            //hotel.GetDestination(1, 500);
            //hotel.GetFacility(1, 1000);
            //hotel.GetFacilityGroup(1, 1000);
            //hotel.GetRoom(1, 1000);
            //hotel.GetSegment(1, 1000);
            //var rate = new GetRateComment();

            //Console.WriteLine("Getting Hotel Detail");
            //hotel.GetHotelData(1, 1000);

            //Console.WriteLine("Update Hotel List by Location Content");
            //service.UpdateHotelListByLocationContent();

            //Console.WriteLine("Update Hotel Detail by Location Content");
            //service.SaveHotelDetailByLocation();

            //Console.WriteLine("RateComment");
            //rate.GetRateCommentData(1, 1000);

            var hotelService = HotelService.GetInstance();
            hotelService.UpdateHotelImage();

            ///*Try to get file name*/
            //var blobService = BlobStorageService.GetInstance();
            //blobService.GetFileNameList("hotelimage");
            stopwatch.Stop();
            Debug.Print("Done in : {0}", stopwatch.Elapsed);
            Console.WriteLine("Done in : {0}", stopwatch.Elapsed);
            Console.ReadKey();
        }
    }
}
