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
        public BcaKlikPay BcaKlikPay { get; set; }
        public CimbClicks CimbClicks { get; set; }
        public CreditCard CreditCard { get; set; }
        public Indomaret Indomaret { get; set; }
        public IndosatDompetku IndosatDompetku { get; set; }
        public MandiriBillPayment MandiriBillPayment { get; set; }
        public MandiriClickPay MandiriClickPay { get; set; }
        public MandiriEcash MandiriEcash { get; set; }
        public TelkomselTcash TelkomselTcash { get; set; }
        public VirtualAccount VirtualAccount { get; set; }
    }
}
