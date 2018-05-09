using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.CustomerWeb.Models
{
    public class FlightCheckoutData
    {
        public string Token { get; set; }
        public FlightItineraryForDisplay Itinerary { get; set; }
        public Contact Contact { get; set; }
        public List<PassengerData> Passengers { get; set; }
        public RsvPaymentDetails PaymentDetailsData { get; set; }
        public string DiscountCode { get; set; }
        public DateTime ExpiryTime { get; set; }
        public List<Pax> SavedPassengers { get; set; }
        public List<SavedCreditCard> SavedCreditCards { get; set; }
    }

    public class PassengerData
    {
        public PaxType Type { get; set; }
        public Title Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
        public string Country { get; set; }
    }
}