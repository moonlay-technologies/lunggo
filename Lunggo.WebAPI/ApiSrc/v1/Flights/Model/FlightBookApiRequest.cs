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
        public Contact Contact { get; set; }
        [JsonProperty("paxs")]
        public List<Passenger> Passengers { get; set; }
        [JsonProperty("pay")]
        public Payment Payment { get; set; }
        [JsonProperty("code")]
        public string DiscountCode { get; set; }
        [JsonProperty("lang")]
        public string Language { get; set; }
    }

    public class Passenger
    {
        [JsonProperty("type")]
        public PassengerType Type { get; set; }
        [JsonProperty("title")]
        public Title Title { get; set; }
        [JsonProperty("first")]
        public string FirstName { get; set; }
        [JsonProperty("last")]
        public string LastName { get; set; }
        [JsonProperty("bd")]
        public DateTime? BirthDate { get; set; }
        [JsonProperty("pass_no")]
        public string PassportNumber { get; set; }
        [JsonProperty("pass_exp")]
        public DateTime? PassportExpiryDate { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}