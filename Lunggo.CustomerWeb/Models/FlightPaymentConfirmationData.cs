using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.CustomerWeb.Models
{
    public class FlightPaymentConfirmationData
    {
        public string RsvNo { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}