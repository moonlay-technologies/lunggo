using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.CustomerWeb.Models
{
    public class FlightThankyouData
    {
        public PaymentStatus Status { get; set; }
        public string RsvNo { get; set; }
    }
}