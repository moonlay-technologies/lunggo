using System;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Activity.Model
{
    public class SearchResultForDisplay
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
        [JsonProperty("shortDesc", NullValueHandling = NullValueHandling.Ignore)]
        public string ShortDesc { get; set; }
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Price { get; set; }
        [JsonProperty("priceDetail", NullValueHandling = NullValueHandling.Ignore)]
        public string PriceDetail { get; set; }
        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public DurationActivity Duration { get; set; }
        [JsonProperty("mediaSrc", NullValueHandling = NullValueHandling.Ignore)]
        public string MediaSrc { get; set; }
    }

    public class SearchResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ShortDesc { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Price { get; set; }
        public string PriceDetail { get; set; }
        public DurationActivity Duration { get; set; }
        public string MediaSrc { get; set; }

    }

    public class DurationActivity
    {
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        public string Amount { get; set; }
        [JsonProperty("unit", NullValueHandling = NullValueHandling.Ignore)]
        public string Unit { get; set; }
    }
}
