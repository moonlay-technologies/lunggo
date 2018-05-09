using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetAvailableDatesApiResponse : ApiResponseBase
    {
        [JsonProperty("availableDateTimes", NullValueHandling = NullValueHandling.Ignore)]
        public List<DateAndAvailableHourApi> AvailableDateTimes { get; set; }
    }

    public class DateAndAvailableHourApi
    {
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }
        [JsonProperty("availableHours", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> AvailableHours { get; set; }
        [JsonProperty("availableSessionAndPaxSlots", NullValueHandling = NullValueHandling.Ignore)]
        public List<AvailableSessionAndPaxSlot> AvailableSessionAndPaxSlots { get; set; }
    }
}