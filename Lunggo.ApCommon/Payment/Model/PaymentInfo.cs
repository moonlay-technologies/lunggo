using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.ApCommon.Payment.Model
{
    public class PaymentInfo
    {
        public string Id { get; set; }
        public PaymentMedium Medium { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime? Time { get; set; }
        public string TargetAccount { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
