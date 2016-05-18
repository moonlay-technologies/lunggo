using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class TopDestinationsApiResponse : ApiResponseBase
    {
        [JsonProperty("topDestinations", NullValueHandling = NullValueHandling.Ignore)]
        public List<TopDestination> TopDestinationList { get; set; }
    }
}