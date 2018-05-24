using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class SetRsvRefundBankAccountApiRequest
    {
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("bankAccount", NullValueHandling = NullValueHandling.Ignore)]
        public BankAccount BankAccount { get; set; }
    }
}