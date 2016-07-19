using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class GetCalendarApiResponse : ApiResponseBase
    {
        [JsonProperty("events")]
        public List<CalendarEvent> CalendarEvents { get; set; }
    }

    public class CalendarEvent
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("type")]
        public Event Event { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public enum Event
    {
        Holiday = 0,
        Other = 99
    }
}