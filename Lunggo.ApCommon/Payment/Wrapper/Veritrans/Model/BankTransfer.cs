using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Wrapper.Veritrans.Model
{
    internal class BankTransfer
    {
        [JsonProperty("bank")]
        public string Bank { get; set; }
    }
}