using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetAvailableDatesApiResponse : ApiResponseBase
    {
        [JsonProperty("availableDateTimes", NullValueHandling = NullValueHandling.Ignore)]
        public List<DateAndAvailableHour> AvailableDateTimes { get; set; }
    }
}