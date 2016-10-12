using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Wrapper.Content;

namespace Lunggo.Webjob.HotelContentWrapper
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Create new stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing.
            stopwatch.Start();
            GetHotel x = new GetHotel();
            x.GetHotelData();
            stopwatch.Stop();
            Debug.Print("Done in : {0}", stopwatch.Elapsed);
        }
    }
}
