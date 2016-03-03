using System;
using Lunggo.ApCommon.Flight.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightPassenger
    {
        [JsonProperty("type")]
        public PassengerType Type { get; set; }
        [JsonProperty("title")]
        public Title Title { get; set; }
        [JsonProperty("first")]
        public string FirstName { get; set; }
        [JsonProperty("last")]
        public string LastName { get; set; }
        [JsonProperty("dob")]
        public DateTime? DateOfBirth { get; set; }
        [JsonProperty("pass_no")]
        public string PassportNumber { get; set; }
        [JsonProperty("gender")]
        public Gender Gender { get; set; }
        [JsonProperty("pass_exp")]
        public DateTime? PassportExpiryDate { get; set; }
        [JsonProperty("pass_country")]
        public string PassportCountry { get; set; }
    }
}