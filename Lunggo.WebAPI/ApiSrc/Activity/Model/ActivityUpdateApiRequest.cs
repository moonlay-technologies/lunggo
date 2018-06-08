using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ActivityUpdateApiRequest
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
        public string Duration { get; set; }
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
}
