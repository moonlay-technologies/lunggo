using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model.Data
{
    public class CreditCard
    {
        [JsonProperty("tokenId")]
        internal string TokenId { get; set; }
        [JsonProperty("bank")]
        public string Bank { get; set; }
        [JsonProperty("holderName")]
        public string HolderName { get; set; }
        [JsonProperty("holderEmail")]
        public string HolderEmail { get; set; }
        [JsonProperty("installmentTerm")]
        public int? InstallmentTerm { get; set; }
        [JsonProperty("bins")]
        public List<string> AllowedBins { get; set; }
        [JsonProperty("saveTokenId")]
        public bool TokenIdSaveEnabled { get; set; }
    }
}