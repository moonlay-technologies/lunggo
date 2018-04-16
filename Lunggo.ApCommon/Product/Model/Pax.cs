using System;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Product.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Product.Model
{
    public class PaxForDisplay
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public PaxType? Type { get; set; }
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public Title? Title { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("dob", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DateOfBirth { get; set; }
        [JsonProperty("nationality", NullValueHandling = NullValueHandling.Ignore)]
        public string Nationality { get; set; }
        [JsonProperty("passportNo", NullValueHandling = NullValueHandling.Ignore)]
        public string PassportNumber { get; set; }
        [JsonProperty("passportExp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PassportExpiryDate { get; set; }
        [JsonProperty("passportCountry", NullValueHandling = NullValueHandling.Ignore)]
        public string PassportCountry { get; set; }
    }

    public class Pax
    {
        [JsonProperty("paxType", NullValueHandling = NullValueHandling.Ignore)]
        public PaxType? Type { get; set; }
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public Title? Title { get; set; }
        [JsonProperty("firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }
        [JsonProperty("lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        [JsonProperty("dateOfBirth", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DateOfBirth { get; set; }
        [JsonProperty("gender", NullValueHandling = NullValueHandling.Ignore)]
        public Gender? Gender { get; set; }
        [JsonProperty("nationality", NullValueHandling = NullValueHandling.Ignore)]
        public string Nationality { get; set; }
        [JsonProperty("passportNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PassportNumber { get; set; }
        [JsonProperty("passportExpiryDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? PassportExpiryDate { get; set; }
        [JsonProperty("passportCountry", NullValueHandling = NullValueHandling.Ignore)]
        public string PassportCountry { get; set; }
    }
}