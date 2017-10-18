using System;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Activity.Model
{

    public class ActivityDetailForDisplay
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int ActivityId { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [JsonProperty("operationTime", NullValueHandling = NullValueHandling.Ignore)]
        public string OperationTime { get; set; }
        [JsonProperty("importantNotice", NullValueHandling = NullValueHandling.Ignore)]
        public string ImportantNotice { get; set; }
        [JsonProperty("warning", NullValueHandling = NullValueHandling.Ignore)]
        public string Warning { get; set; }
        [JsonProperty("additionalNotes", NullValueHandling = NullValueHandling.Ignore)]
        public string AdditionalNotes { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Price { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }

    }
    public class ActivityDetail
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string OperationTime { get; set; }
        public string ImportantNotice { get; set; }
        public string Warning { get; set; }
        public string AdditionalNotes { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
    
}
