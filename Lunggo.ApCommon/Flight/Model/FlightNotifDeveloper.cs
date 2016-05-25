using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightNotifDeveloper
    {
        public string RsvNo { get; set; }
        public List<FlightIssueData> FlightIssueData { get; set; }
    }
}
