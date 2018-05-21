using System.Collections.Generic;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class GetUserBankAccountApiResponse : ApiResponseBase
    {
        [JsonProperty("bankAccounts", NullValueHandling = NullValueHandling.Ignore)]
        public List<BankAccount> BankAccounts { get; set; }
    }
}