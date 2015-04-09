using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Payment.Data;

namespace Lunggo.BackendWeb.Model
{
    public class TestInputData
    {
        public string FareId { get; set; }
        public decimal TotalFare { get; set; }
        public FlightFareItinerary Itinerary { get; set; }
        public string ReturnFareId { get; set; }
        public decimal ReturnTotalFare { get; set; }
        public FlightFareItinerary ReturnItinerary { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public List<PassengerFareInfo> PassengerData { get; set; }
        public PaymentData PaymentData { get; set; }
    }
}