using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class GetAccountStatementApiRequest
    {
        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public string FromDate { get; set; }
        [JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        public string ToDate { get; set; }
    }
}