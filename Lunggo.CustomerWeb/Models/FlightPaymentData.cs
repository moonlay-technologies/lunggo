using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.CustomerWeb.Models
{
    public class FlightPaymentData
    {
        public string RsvNo { get; set; }
        public FlightReservationForDisplay Reservation { get; set; }
        public DateTime? TimeLimit { get; set; }
        public List<SavedCreditCard> SavedCreditCards { get; set; }
    }
}