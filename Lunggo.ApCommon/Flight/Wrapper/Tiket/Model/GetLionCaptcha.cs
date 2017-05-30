using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Wrapper.Tiket.Model
{
    //    Specially for Lion air flights, you must request a captcha first.
    //    If it exists, the captcha and sessionid must be included while calling add order.
    public class GetLionCaptcha : TiketBaseResponse
    {
        [JsonProperty("lioncaptcha", NullValueHandling = NullValueHandling.Ignore)]
        public string Lioncaptcha { get; set; }
        [JsonProperty("lionsessionid", NullValueHandling = NullValueHandling.Ignore)]
        public string Lionsessionid { get; set; }

    }
}
