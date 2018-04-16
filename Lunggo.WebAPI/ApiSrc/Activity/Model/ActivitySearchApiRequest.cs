using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ActivitySearchApiRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("startDate")]
        public string StartDate { get; set; }
        [JsonProperty("endDate")]
        public string EndDate { get; set; }
        [JsonProperty("page")]
        public string Page { get; set; }
        [JsonProperty("perPage")]
        public string PerPage { get; set; }
    }
}
