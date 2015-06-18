using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class PaymentExpiry
    {
        [JsonProperty("expiry_duration")]
        public int Duration { get; set; }
        [JsonProperty("unit")]
        public string Unit { get; set; }
    }
}
