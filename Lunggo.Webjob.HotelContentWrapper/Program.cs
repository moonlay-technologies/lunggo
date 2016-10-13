using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Wrapper.Content;
using Lunggo.Framework.Documents;

namespace Lunggo.Webjob.HotelContentWrapper
{
    public class Program
    {
        static void Main(string[] args)
        {
            DocumentService.GetInstance().Init("https://travorama-development-docdb.documents.azure.com:443/", "64nWp0NH3XWQ2TGepOWrhqKQFBG4B2VeixUfnyfPqwpEAmLlvUx2ZyqvAwvlFluvDNnk7ofQXYQ8G6wbQPLwkw==", "travorama-local", "travorama-local");
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
