using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Wrapper.Veritrans.Model
{
    internal class Indomaret
    {
        [JsonProperty("Store")]
        public string StoreName { get; set; }
        [JsonProperty("Message")]
        public string Label { get; set; }
    }
}