using System.Net;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Voucher.Model
{
    public class CheckVoucherApiResponse
    {
        [JsonProperty("discount")]
        public decimal Discount { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        [JsonProperty("original_request")]
        public CheckVoucherApiRequest OriginalRequest { get; set; }
    }
}