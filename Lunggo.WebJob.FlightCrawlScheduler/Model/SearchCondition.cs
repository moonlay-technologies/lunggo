using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.WebJob.FlightCrawlScheduler.Model
{
    internal class SearchCondition
    {
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public CabinClass RequestedCabinClass { get; set; }
        public string OriginAirport { get; set; }
        public string DestinationAirport { get; set; }
        public int DaysAdvanceDepartureDateStart { get; set; }
        public int DaysAdvanceDepartureDateEnd { get; set; }
        public int Timeout { get; set; }
    }
}
