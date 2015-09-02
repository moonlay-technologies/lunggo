using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.CustomerWeb.Models
{
    public class FlightPaymentConfirmationData
    {
        public string RsvNo { get; set; }
        public decimal FinalPrice { get; set; }
        public DateTime TimeLimit { get; set; }
        public TransferConfirmationReport Report { get; set; }
    }
}