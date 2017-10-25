using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Activity.Model
{

    public class ActivityDetailForDisplay
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? ActivityId { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("shortDesc", NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDesc { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
        [JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Latitude { get; set; }
        [JsonProperty("long", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Longitude { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Price { get; set; }
        [JsonProperty("operationTime", NullValueHandling = NullValueHandling.Ignore)]
        public string OperationTime { get; set; }
        [JsonProperty("mediaSrc", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> MediaSrc { get; set; }
        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public List<ContentActivityDetail> Content { get; set; }
        [JsonProperty("additionalContent", NullValueHandling = NullValueHandling.Ignore)]
        public List<AdditionalContent> AdditionalContent { get; set; }
        [JsonProperty("cancellation", NullValueHandling = NullValueHandling.Ignore)]
        public string Cancellation { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Date { get; set; }

    }
    public class ActivityDetail
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal Price { get; set; }
        public List<string> MediaSrc { get; set; }
        public string OperationTime { get; set; }
        public List<ContentActivityDetail> Content { get; set; }
        public List<AdditionalContent> AdditionalContent { get; set; }
        public string Cancellation { get; set; }
        public DateTime Date { get; set; }
    }

    public class ContentActivityDetail
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty("desc", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }

    public class AdditionalContent
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public List<ContentActivityDetail> Content { get; set; }
    }
}
