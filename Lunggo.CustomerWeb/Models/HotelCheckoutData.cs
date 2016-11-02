using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.CustomerWeb.Models
{
    public class HotelCheckoutData
    {
        public string Token { get; set; }
        public HotelDetailForDisplay HotelDetail { get; set; }
        public Contact Contact { get; set; }
        public List<PassengerData> Passengers { get; set; }
        public PaymentDetails PaymentDetailsData { get; set; }
        public string DiscountCode { get; set; }
        public DateTime ExpiryTime { get; set; }
        public List<Pax> SavedPassengers { get; set; }
        public List<SavedCreditCard> SavedCreditCards { get; set; }
    }

}