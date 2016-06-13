using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class TransferFeeApiResponse : ApiResponseBase
    {
        [JsonProperty("fee", NullValueHandling = NullValueHandling.Ignore)]
        public decimal TransferFee { get; set; }
    }
}