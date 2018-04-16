using System.Collections.Generic;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class GetAccountStatementApiResponse : ApiResponseBase
    {
        [JsonProperty("transactions", NullValueHandling = NullValueHandling.Ignore)]
        public List<Transaction> Transactions { get; set; }
    }
}