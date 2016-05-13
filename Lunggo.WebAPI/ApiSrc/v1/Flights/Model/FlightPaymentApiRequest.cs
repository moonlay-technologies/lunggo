using Lunggo.ApCommon.Payment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightPaymentApiRequest
    {
        public string RsvNo { get; set; }
        public PaymentInfo Payment { get; set; }
    }
}