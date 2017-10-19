using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetAvailableDatesApiResponse : ApiResponseBase
    {
        [JsonProperty("availableDates", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityDetailForDisplay> AvailableDates { get; set; }
    }
}