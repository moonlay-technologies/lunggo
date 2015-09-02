using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRulesApiResponse
    {
        public List<AirlineRules> AirlineRules { get; set; }
        public List<BaggageRules> BaggageRules { get; set; }
        public FlightRulesApiRequest OriginalRequest { get; set; }
    }
}