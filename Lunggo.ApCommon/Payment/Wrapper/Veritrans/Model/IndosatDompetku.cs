using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Wrapper.Veritrans.Model
{
    internal class IndosatDompetku
    {
        [JsonProperty("msisdn")]
        public string PhoneNumber { get; set; }
    }
}