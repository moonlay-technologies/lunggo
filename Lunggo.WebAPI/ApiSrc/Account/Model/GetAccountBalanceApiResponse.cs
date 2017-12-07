using System.Collections.Generic;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class GetAccountBalanceApiResponse : ApiResponseBase
    {
        [JsonProperty("balance", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Balance { get; set; }
        [JsonProperty("withdrawable", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Withdrawable { get; set; }
    }
}