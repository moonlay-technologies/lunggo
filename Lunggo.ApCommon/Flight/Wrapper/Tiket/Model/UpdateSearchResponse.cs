using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket.Model
{
    public class UpdateSearchResponse
    {
        [JsonProperty("diagnostic", NullValueHandling = NullValueHandling.Ignore)]
        public Diagnostic Diagnostic { get; set; }
        [JsonProperty("login_status", NullValueHandling = NullValueHandling.Ignore)]
        public string LoginStatus { get; set; }

        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }
        [JsonProperty("update", NullValueHandling = NullValueHandling.Ignore)]
        public int Update { get; set; }
    }
}
