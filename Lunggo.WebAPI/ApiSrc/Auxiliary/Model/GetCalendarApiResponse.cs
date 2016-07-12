using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Common.Model;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class GetCalendarApiResponse : ApiResponseBase
    {
        public List<CalendarEvent> CalendarEvents { get; set; }
    }

    public class CalendarEvent
    {
        public DateTime Date { get; set; }
        public Event Event { get; set; }
        public string Name { get; set; }
    }

    public enum Event
    {
        Holiday = 0,
        Other = 99
    }
}