using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Service;
using Lunggo.ApCommon.Hotel.Wrapper.Content;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content;
using Lunggo.Framework.Documents;
using Lunggo.Framework.TableStorage;

namespace Lunggo.Webjob.HotelContentWrapper
{
    public class Program
    {
        static void Main(string[] args)
        {
            //DocumentService.GetInstance().Init("https://travorama-development-docdb.documents.azure.com:443/", "64nWp0NH3XWQ2TGepOWrhqKQFBG4B2VeixUfnyfPqwpEAmLlvUx2ZyqvAwvlFluvDNnk7ofQXYQ8G6wbQPLwkw==", "travorama-local", "travorama-local");
            TableStorageService.GetInstance().Init("DefaultEndpointsProtocol=https;AccountName=travoramalocal;AccountKey=t9BOHU0NktEB4qvBd7eSdXtSYabT/wDxnC2PndRtDNdQWymLUko6q0oKGICBZ0FoX7GLvGV9v4QSNYZPu98ZWw==");
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            var service = HotelService.GetInstance();
            var hotel = new GetHotel();
            var rate = new GetRateComment();

            //HotelService.GetInstance().SaveTruncatedHotelDetail();
            //HotelService.GetInstance().UpdateHotelAmenitiesContent();
            
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