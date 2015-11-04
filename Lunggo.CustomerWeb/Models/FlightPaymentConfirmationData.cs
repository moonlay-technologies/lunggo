using System;
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