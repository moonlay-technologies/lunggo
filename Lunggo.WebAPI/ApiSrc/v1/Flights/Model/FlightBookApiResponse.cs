using System;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightBookApiResponse
    {
        public bool IsSuccess { get; set; }
        public string RsvNo { get; set; }
        public string PaymentUrl { get; set; }
        public DateTime? TimeLimit { get; set; }
        public FlightError Error { get; set; }
        public FlightBookApiRequest OriginalRequest { get; set; }
    }
}