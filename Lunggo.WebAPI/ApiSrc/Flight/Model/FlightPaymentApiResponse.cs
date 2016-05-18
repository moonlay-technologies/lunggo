using Lunggo.ApCommon.Flight.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightPaymentApiResponse
    {
        public bool IsSuccess { get; set; }
        public string RsvNo { get; set; }
        public FlightError Error { get; set; }
        public FlightPaymentApiRequest OriginalRequest { get; set; }
    }
}