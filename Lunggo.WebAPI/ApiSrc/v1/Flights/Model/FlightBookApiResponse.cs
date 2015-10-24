using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightBookApiResponse
    {
        public bool IsSuccess { get; set; }
        public string RsvNo { get; set; }
        public string PaymentUrl { get; set; }
        public DateTime? TimeLimit { get; set; }
        public FlightBookApiRequest OriginalRequest { get; set; }
    }
}