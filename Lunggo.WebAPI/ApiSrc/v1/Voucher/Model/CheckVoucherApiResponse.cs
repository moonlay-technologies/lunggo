﻿using System.Net;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Voucher.Model
{
    public class CheckVoucherApiResponse : ApiResponseBase
    {
        [JsonProperty("disc")]
        public decimal Discount { get; set; }
        [JsonProperty("disp")]
        public string DisplayName { get; set; }
    }
}