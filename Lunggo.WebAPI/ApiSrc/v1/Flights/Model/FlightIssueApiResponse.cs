using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightIssueApiResponse
    {
        public bool IsSuccess { get; set; }
        public FlightIssueApiRequest OriginalRequest { get; set; }
    }
}