using System.Collections.Generic;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ActivityBookApiRequest
    {
        [JsonProperty("id")]
        public string ActivityId { get; set; }
        [JsonProperty("contact")]
        public Contact Contact { get; set; }
        [JsonProperty("pax")]
        public List<PaxForDisplay> Passengers { get; set; }
        [JsonProperty("lang")]
        public string LanguageCode { get; set; }
    }
}