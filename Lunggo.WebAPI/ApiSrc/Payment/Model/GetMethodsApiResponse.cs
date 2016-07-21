using System.Collections.Generic;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Payment.Model
{
    public class GetMethodsApiResponse : ApiResponseBase
    {
        [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        public List<Option> Options { get; set; }
    }

    public class Option
    {
        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentMethod? Method { get; set; }
        [JsonProperty("banks", NullValueHandling = NullValueHandling.Ignore)]
        public List<BankDetails> Banks { get; set; }
        [JsonProperty("available", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Available { get; set; }
        [JsonProperty("startHour", NullValueHandling = NullValueHandling.Ignore)]
        public int? AvailableStartHour { get; set; }
        [JsonProperty("startMinute", NullValueHandling = NullValueHandling.Ignore)]
        public int? AvailableStartMinute { get; set; }
        [JsonProperty("endHour", NullValueHandling = NullValueHandling.Ignore)]
        public int? AvailableEndHour { get; set; }
        [JsonProperty("endMinute", NullValueHandling = NullValueHandling.Ignore)]
        public int? AvailableEndMinute { get; set; }
    }

    public class BankDetails
    {
        [JsonProperty("bank", NullValueHandling = NullValueHandling.Ignore)]
        public Bank? Bank { get; set; }
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }
        [JsonProperty("branch", NullValueHandling = NullValueHandling.Ignore)]
        public string Branch { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("logoUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string LogoUrl { get; set; }
        [JsonProperty("available", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Available { get; set; }
        [JsonProperty("startHour", NullValueHandling = NullValueHandling.Ignore)]
        public int? AvailableStartHour { get; set; }
        [JsonProperty("startMinute", NullValueHandling = NullValueHandling.Ignore)]
        public int? AvailableStartMinute { get; set; }
        [JsonProperty("endHour", NullValueHandling = NullValueHandling.Ignore)]
        public int? AvailableEndHour { get; set; }
        [JsonProperty("endMinute", NullValueHandling = NullValueHandling.Ignore)]
        public int? AvailableEndMinute { get; set; }
    }

    public enum Bank
    {
        Mandiri = 0,
        Bca = 1,
        Bni = 2,
        Bri = 3,
        CimbNiaga = 4
    }
}