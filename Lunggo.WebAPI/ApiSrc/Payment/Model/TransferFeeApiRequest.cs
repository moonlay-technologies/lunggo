﻿using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class TransferFeeApiRequest
    {
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
        [JsonProperty("discCode")]
        public string DiscountCode { get; set; }
    }
}