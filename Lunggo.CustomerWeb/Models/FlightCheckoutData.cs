using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.CustomerWeb.Models
{
    public class FlightCheckoutData
    {
        public string HashKey { get; set; }
        public FlightItineraryApi ItineraryApi { get; set; }
        public FlightItinerary Itinerary { get; set; }
        public ContactData Contact { get; set; }
        public List<PassengerData> Passengers { get; set; }
        public PaymentInfo Payment { get; set; }
        public string DiscountCode { get; set; }
        public string Message { get; set; }
    }

    public class PassengerData
    {
        public PassengerType Type { get; set; }
        public Title Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
        public string Country { get; set; }
    }
}