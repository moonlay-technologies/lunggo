using Lunggo.ApCommon.Payment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class PayFlightInput
    {

        public string RsvNo { get; set; }
        public PaymentInfo Payment { get; set; }
    }
}
