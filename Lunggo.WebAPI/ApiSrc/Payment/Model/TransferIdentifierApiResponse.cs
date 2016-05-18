using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class TransferIdentifierApiResponse : ApiResponseBase
    {
        [JsonProperty("fee", NullValueHandling = NullValueHandling.Ignore)]
        public int TransferFee { get; set; }

    }
}