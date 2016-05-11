using System.Net;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Payment.Model
{
    public class TransferIdentifierApiResponse : ApiResponseBase
    {
        [JsonProperty("fee", NullValueHandling = NullValueHandling.Ignore)]
        public int TransferFee { get; set; }

    }
}