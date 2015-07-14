using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Payment.Data;

namespace Lunggo.CustomerWeb.Models
{
    public class FlightCheckoutData
    {
        public string HashKey { get; set; }
        public FlightItineraryApi Itinerary { get; set; }
        public FlightItineraryFare ItineraryFare { get; set; }
        public ContactData ContactData { get; set; }
        public List<PassengerData> AdultPassengerData { get; set; }
        public List<PassengerData> ChildPassengerData { get; set; }
        public List<PassengerData> InfantPassengerData { get; set; }
        public PaymentData PaymentData { get; set; }
        public string Message { get; set; }
    }

    public class PassengerData
    {
        public Title Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
        public string Country { get; set; }
    }
}