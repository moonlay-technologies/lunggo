using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Lunggo.ApCommon.Flight.Model
{
    public class GetRulesResult : ResultBase
    {
        public List<AirlineRules> AirlineRules { get; set; }
        public List<BaggageRules> BaggageRules { get; set; }

        public GetRulesResult()
        {
            AirlineRules = new List<AirlineRules>();
            BaggageRules = new List<BaggageRules>();
        }
    }

    public class AirlineRules
    {
        public string AirlineCode { get; set; }
        public string AirlineName { get; set; }
        public string DepartureAirport { get; set; }
        public string DepartureAirportName { get; set; }
        public string ArrivalAirport { get; set; }
        public string ArrivalAirportName { get; set; }
        public List<string> Rules { get; set; }
    }

    public class BaggageRules
    {
        public string AirlineCode { get; set; }
        public string AirlineName { get; set; }
        public string FlightNumber { get; set; }
        public string DepartureAirport { get; set; }
        public string DepartureAirportName { get; set; }
        public string ArrivalAirport { get; set; }
        public string ArrivalAirportName { get; set; }
        public string Baggage { get; set; }
    }
}
