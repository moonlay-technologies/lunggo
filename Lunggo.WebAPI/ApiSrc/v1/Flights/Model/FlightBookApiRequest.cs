using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightBookApiRequest
    {
        [JsonProperty("tkn")]
        public string Token { get; set; }
        [JsonProperty("con")]
        public Contact Contact { get; set; }
        [JsonProperty("pax")]
        public List<Passenger> Passengers { get; set; }
        [JsonProperty("pay")]
        public Payment Payment { get; set; }
        [JsonProperty("cd")]
        public string DiscountCode { get; set; }
        [JsonProperty("lang")]
        public string Language { get; set; }
    }

    public class Passenger
    {
        [JsonProperty("typ")]
        public PassengerType Type { get; set; }
        [JsonProperty("tit")]
        public Title Title { get; set; }
        [JsonProperty("fst")]
        public string FirstName { get; set; }
        [JsonProperty("lst")]
        public string LastName { get; set; }
        [JsonProperty("dob")]
        public DateTime? BirthDate { get; set; }
        [JsonProperty("passno")]
        public string PassportNumber { get; set; }
        [JsonProperty("passexp")]
        public DateTime? PassportExpiryDate { get; set; }
        [JsonProperty("passcty")]
        public string PassportCountry { get; set; }
        [JsonProperty("nat")]
        public string Nationality { get; set; }
    }

    public class Payment
    {
        [JsonProperty("met")]
        public PaymentMethod Method { get; set; }
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Data Data { get; set; }
        [JsonProperty("curr")]
        public string Currency { get; set; }
    }
}