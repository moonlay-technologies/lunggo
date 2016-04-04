using System;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Voucher.Model
{
    public class CheckVoucherApiRequest
    {
        [JsonProperty("cd")]
        public string Code { get; set; }
        [JsonProperty("em")]
        public string Email { get; set; }
        [JsonProperty("tkn")]
        public string Token { get; set; }
    }
}