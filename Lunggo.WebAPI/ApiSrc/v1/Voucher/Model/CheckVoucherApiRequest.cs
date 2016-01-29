using System;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Voucher.Model
{
    public class CheckVoucherApiRequest
    {
        [JsonProperty("code")]
        public String Code { get; set; }
        [JsonProperty("email")]
        public String Email { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}