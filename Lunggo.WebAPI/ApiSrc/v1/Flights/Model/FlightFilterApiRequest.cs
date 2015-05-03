using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightFilterApiRequest
    {
        public string SearchId { get; set; }
        public List<string> IncludedAirlines { get; set; }
        public List<DateTimeSpan> DepartureTimeSpans { get; set; }
        public List<DateTimeSpan> ArrivalTimeSpans { get; set; }
        public PriceRange PriceRange { get; set; }
        public List<int> TotalTransits { get; set; }
    }

    public class DateTimeSpan
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class PriceRange
    {
        public decimal Start { get; set; }
        public decimal End { get; set; }
    }
}