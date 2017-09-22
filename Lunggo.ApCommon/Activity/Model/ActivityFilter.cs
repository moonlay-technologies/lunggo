using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Activity.Model
{

    public class ActivityFilter
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public PriceFilter Price { get; set; }
    }

    public class PriceFilter
    {
        [JsonProperty("min")]
        public decimal? Min { get; set; }
        [JsonProperty("max")]
        public decimal? Max { get; set; }
    }

}
