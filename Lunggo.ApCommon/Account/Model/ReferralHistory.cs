using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Account.Model
{
    public class ReferralHistoryModel
    {
        public string UserId { get; set; }
        public string ReferreeId { get; set; }
        public string History { get; set; }
        public decimal ReferralCredit { get; set; }
        public DateTime? TimeStamp { get; set; }
    }

    public class ReferralHistoryModelForDisplay
    {
        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public string History { get; set; }
        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public string User { get; set; }
        [JsonProperty("referralCredit", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? ReferralCredit { get; set; }
        [JsonProperty("dateTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DateTime { get; set; }
    }
    
}
