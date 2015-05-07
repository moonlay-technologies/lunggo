using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Veritrans.Model
{
    internal class Response
    {
        [JsonProperty("status_code")]
        public string StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        [JsonProperty("redirect_url")]
        public string RedirectUrl { get; set; }
    }
}
