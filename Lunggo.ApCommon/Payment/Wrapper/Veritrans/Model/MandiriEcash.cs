using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Wrapper.Veritrans.Model
{
    internal class MandiriEcash
    {
        [JsonProperty("bank")]
        public string Bank { get; set; }
    }
}