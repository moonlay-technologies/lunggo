using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.ApCommon.Payment.Model
{
    public class Surcharge
    {
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentSubMethod PaymentSubMethod { get; set; }
        public decimal Percentage { get; set; }
        public decimal Constant { get; set; }
    }
}
