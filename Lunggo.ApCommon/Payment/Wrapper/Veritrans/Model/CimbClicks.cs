using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Wrapper.Veritrans.Model
{
    internal class CimbClicks
    {
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}