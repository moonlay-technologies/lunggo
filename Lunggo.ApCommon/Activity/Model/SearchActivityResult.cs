using System;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Activity.Model
{
    public class SearchResultForDisplay
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public string Category { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Price { get; set; }
        [JsonProperty("imgSrc", NullValueHandling = NullValueHandling.Ignore)]
        public string ImgSrc { get; set; }
    }

    public class SearchResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Price { get; set; }
        public string ImgSrc { get; set; }
    }
}
