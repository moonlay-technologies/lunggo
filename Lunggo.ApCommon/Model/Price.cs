using System;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Model
{
    public class Price
    {
        [JsonProperty("val")]
        public decimal Value { get; set; }
        [JsonProperty("currency")]
        public String Currency { get; set; }
    }
}
