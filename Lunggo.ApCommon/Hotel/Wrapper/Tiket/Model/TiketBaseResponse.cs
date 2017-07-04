using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Wrapper.Tiket.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.Tiket.Model
{
    public class TiketHotelBaseResponse
    {
        [JsonProperty("diagnostic", NullValueHandling = NullValueHandling.Ignore)]
        public HotelDiagnostic Diagnostic { get; set; }

        [JsonProperty("login_status", NullValueHandling = NullValueHandling.Ignore)]
        public string LoginStatus { get; set; }

        [JsonProperty("output_type", NullValueHandling = NullValueHandling.Ignore)]
        public string OutputType { get; set; }

        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        [JsonProperty("next_checkout_uri", NullValueHandling = NullValueHandling.Ignore)]
        public string NextCheckoutUri { get; set; }

        [JsonProperty("guest_id", NullValueHandling = NullValueHandling.Ignore)]
        public string GuestId { get; set; }
    }

    public class HotelDiagnostic
    {
        [JsonProperty("confirm", NullValueHandling = NullValueHandling.Ignore)]
        public string Confirm { get; set; }

        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty("elapsetime", NullValueHandling = NullValueHandling.Ignore)]
        public string ElapseTime { get; set; }

        [JsonProperty("lang", NullValueHandling = NullValueHandling.Ignore)]
        public string Language { get; set; }

        [JsonProperty("memoryUsage", NullValueHandling = NullValueHandling.Ignore)]
        public string MemoryUsage { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("unix_timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public string UnixTimeStamp { get; set; }
    }
}
