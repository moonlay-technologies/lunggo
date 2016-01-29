using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightBookApiRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("contact")]
        public ContactData Contact { get; set; }
        [JsonProperty("passengers")]
        public List<PassengerData> Passengers { get; set; }
        [JsonProperty("payment")]
        public PaymentInfo Payment { get; set; }
        [JsonProperty("voucher_code")]
        public string DiscountCode { get; set; }
        [JsonProperty("lang")]
        public string Language { get; set; }
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