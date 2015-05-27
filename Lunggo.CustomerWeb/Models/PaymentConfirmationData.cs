using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.CustomerWeb.Models
{
    public class PaymentConfirmationData
    {
        public string Remitter { get; set; }
        public decimal Amount { get; set; }
        public string RemitterBank { get; set; }
        public string BeneficiaryBank { get; set; }
        public string Message { get; set; }
    }
}