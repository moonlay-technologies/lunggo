using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Wrapper.Veritrans.Model
{
    internal class CreditCard
    {
        [JsonProperty("token_id")]
        internal string TokenId { get; set; }
        [JsonProperty("bank")]
        public string Bank { get; set; }
        [JsonProperty("installment_term")]
        public int? InstallmentTerm { get; set; }
        [JsonProperty("bins")]
        public List<string> AllowedBins { get; set; }
        //[JsonProperty("type")]
        //public string Type { get; set; }
        [JsonProperty("save_token_id")]
        public bool TokenIdSaveEnabled { get; set; }
    }
}