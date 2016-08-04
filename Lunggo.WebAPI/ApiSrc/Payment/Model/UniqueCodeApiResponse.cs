using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class UniqueCodeApiResponse : ApiResponseBase
    {
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public decimal UniqueCode { get; set; }
    }
}