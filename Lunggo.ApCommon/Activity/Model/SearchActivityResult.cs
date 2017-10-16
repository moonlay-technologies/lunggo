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
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        [JsonProperty("operationTime", NullValueHandling = NullValueHandling.Ignore)]
        public string OperationTime { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Price { get; set; }
        [JsonProperty("closeDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CloseDate { get; set; }
        [JsonProperty("imgSrc", NullValueHandling = NullValueHandling.Ignore)]
        public string ImgSrc { get; set; }
    }

    public class SearchResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string OperationTime { get; set; }
        public decimal Price { get; set; }
        public DateTime CloseDate { get; set; }
        public string ImgSrc { get; set; }
    }
}
