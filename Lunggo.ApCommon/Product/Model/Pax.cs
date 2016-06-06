using System;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Product.Constant;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Product.Model
{
    public class Pax
    {
        [JsonProperty("typ")]
        public PaxType Type { get; set; }
        [JsonProperty("tit")]
        public Title Title { get; set; }
        [JsonProperty("fst")]
        public string FirstName { get; set; }
        [JsonProperty("lst")]
        public string LastName { get; set; }
        [JsonProperty("dob")]
        public DateTime? DateOfBirth { get; set; }
        [JsonProperty("gen")]
        public Gender Gender { get; set; }
        [JsonProperty("nat")]
        public string Nationality { get; set; }
        [JsonProperty("passno")]
        public string PassportNumber { get; set; }
        [JsonProperty("passexp")]
        public DateTime? PassportExpiryDate { get; set; }
        [JsonProperty("passcty")]
        public string PassportCountry { get; set; }
    }
}