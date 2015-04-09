using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightSearchApiRequest
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime? Date { get; set; }
        public string Cabin { get; set; }
        public int? Adult { get; set; }
        public int? Child { get; set; }
        public int? Infant { get; set; }
    }
}