using System.Collections.Generic;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ActivityBookApiRequest
    {
        [JsonProperty("activityId", NullValueHandling = NullValueHandling.Ignore)]
        public string ActivityId { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }
        [JsonProperty("selectedSession", NullValueHandling = NullValueHandling.Ignore)]
        public string SelectedSession { get; set; }
        [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
        public Contact Contact { get; set; }
        [JsonProperty("pax", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaxForDisplay> Passengers { get; set; }
        [JsonProperty("ticketCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? TicketCount { get; set; }
    }
}