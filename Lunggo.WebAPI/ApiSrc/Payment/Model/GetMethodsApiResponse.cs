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
}