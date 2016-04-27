using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.CustomerWeb.Models
{
    public class FlightPaymentData
    {
        //public string Token { get; set; }
        public string RsvNo { get; set; }
        public FlightReservationForDisplay Reservation { get; set; }
        public PaymentInfo Payment { get; set; }
        public List<SavedCreditCard> SavedCreditCards { get; set; }
    }
}