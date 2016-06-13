using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model.Data;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Payment.Model
{
    public class PaymentData
    {
        [JsonProperty("bcaKlikPay", NullValueHandling = NullValueHandling.Ignore)]
        public BcaKlikPay BcaKlikPay { get; set; }
        [JsonProperty("cimbClicks", NullValueHandling = NullValueHandling.Ignore)]
        public CimbClicks CimbClicks { get; set; }
        [JsonProperty("creditCard", NullValueHandling = NullValueHandling.Ignore)]
        public CreditCard CreditCard { get; set; }
        [JsonProperty("indomaret", NullValueHandling = NullValueHandling.Ignore)]
        public Indomaret Indomaret { get; set; }
        [JsonProperty("indosatDompetku", NullValueHandling = NullValueHandling.Ignore)]
        public IndosatDompetku IndosatDompetku { get; set; }
        [JsonProperty("mandiriBillPayment", NullValueHandling = NullValueHandling.Ignore)]
        public MandiriBillPayment MandiriBillPayment { get; set; }
        [JsonProperty("mandiriClickPay", NullValueHandling = NullValueHandling.Ignore)]
        public MandiriClickPay MandiriClickPay { get; set; }
        [JsonProperty("mandiriEcash", NullValueHandling = NullValueHandling.Ignore)]
        public MandiriEcash MandiriEcash { get; set; }
        [JsonProperty("telkomselTcash", NullValueHandling = NullValueHandling.Ignore)]
        public TelkomselTcash TelkomselTcash { get; set; }
        [JsonProperty("virtualAccount", NullValueHandling = NullValueHandling.Ignore)]
        public VirtualAccount VirtualAccount { get; set; }
    }
}
