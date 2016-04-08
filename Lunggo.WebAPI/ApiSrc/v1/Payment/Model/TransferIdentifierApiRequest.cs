using System.Net;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Payment.Model
{
    public class TransferIdentifierApiRequest
    {
        [JsonProperty("pr")]
        public decimal Price { get; set; }

    }
}