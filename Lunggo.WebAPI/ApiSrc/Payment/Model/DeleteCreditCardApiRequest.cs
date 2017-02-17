using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class DeleteCreditCardApiRequest
    {
        [JsonProperty("maskedCardNo")]
        public string MaskedCardNumber { get; set; }
    }
}