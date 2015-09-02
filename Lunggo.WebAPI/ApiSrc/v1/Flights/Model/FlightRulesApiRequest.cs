using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRulesApiRequest
    {
        public string SearchId { get; set; }
        public int ItinIndex { get; set; }
    }
}