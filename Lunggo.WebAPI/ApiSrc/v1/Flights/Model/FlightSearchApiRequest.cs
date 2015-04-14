using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightSearchApiRequest
    {
        public string Ori { get; set; }
        public string Dest { get; set; }
        public DateTime Date { get; set; }
        public string Cabin { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Infant { get; set; }
        public string Type { get; set; }
    }

    public class OriDestDate
    {
        public string Ori { get; set; }
        public string Dest { get; set; }
        public DateTime Date { get; set; }
    }
}