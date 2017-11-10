using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Activity.Model
{

    public class ActivityDetailForDisplay
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long ActivityId { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
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
        [JsonProperty("priceDetail", NullValueHandling = NullValueHandling.Ignore)]
        public string PriceDetail { get; set; }
        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public DurationActivity Duration { get; set; }
        [JsonProperty("operationTime", NullValueHandling = NullValueHandling.Ignore)]
        public string OperationTime { get; set; }
        [JsonProperty("mediaSrc", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> MediaSrc { get; set; }
        [JsonProperty("contents", NullValueHandling = NullValueHandling.Ignore)]
        public Content Contents { get; set; }
        [JsonProperty("additionalContent", NullValueHandling = NullValueHandling.Ignore)]
        public List<AdditionalContent> AdditionalContent { get; set; }
        [JsonProperty("cancellation", NullValueHandling = NullValueHandling.Ignore)]
        public string Cancellation { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Date { get; set; }
        [JsonProperty("requiredPaxData", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> RequiredPaxData { get; set; }

    }
    public class ActivityDetail
    {
        public long ActivityId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ShortDesc { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal Price { get; set; }
        public string PriceDetail { get; set; }
        public DurationActivity Duration { get; set; }
        public string OperationTime { get; set; }
        public List<string> MediaSrc { get; set; }
        public Content Contents { get; set; }
        public List<AdditionalContent> AdditionalContent { get; set; }
        public string Cancellation { get; set; }
        public DateTime Date { get; set; }
        public bool IsPassportNumberNeeded { get; set; }
        public bool IsPassportIssuedDateNeeded { get; set; }
        public bool IsPaxDoBNeeded { get; set; }
    }

    public class AdditionalContent
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
        [JsonProperty("desc", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }

    public class Content
    {
        [JsonProperty("content1", NullValueHandling = NullValueHandling.Ignore)]
        public string Content1 { get; set; }
        [JsonProperty("content2", NullValueHandling = NullValueHandling.Ignore)]
        public string Content2 { get; set; }
        [JsonProperty("content3", NullValueHandling = NullValueHandling.Ignore)]
        public string Content3 { get; set; }
    }

    public class DateAndAvailableHour
    {
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Date { get; set; }
        [JsonProperty("availableHour", NullValueHandling = NullValueHandling.Ignore)]
        public string AvailableHour { get; set; }
    }
}
