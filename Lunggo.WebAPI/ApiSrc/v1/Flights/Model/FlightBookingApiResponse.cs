using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightBookingApiResponse
    {
        public string Result { get; set; }
        public string BookingId { get; set; }
        public DateTime? TimeLimit { get; set; }
        public FlightBookingApiRequest OriginalRequest { get; set; }
    }
}