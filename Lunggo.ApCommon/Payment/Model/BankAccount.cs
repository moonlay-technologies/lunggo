using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class BankAccount
    {
        [JsonProperty("accountNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountNumber { get; set; }
        [JsonProperty("bankName", NullValueHandling = NullValueHandling.Ignore)]
        public string BankName { get; set; }
        [JsonProperty("branch", NullValueHandling = NullValueHandling.Ignore)]
        public string Branch { get; set; }
        [JsonProperty("ownerName", NullValueHandling = NullValueHandling.Ignore)]
        public string OwnerName { get; set; }
    }
}
