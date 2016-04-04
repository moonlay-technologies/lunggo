using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class TopDestinationsApiResponse : ApiResponseBase
    {
        [JsonProperty("top", NullValueHandling = NullValueHandling.Ignore)]
        public List<TopDestination> TopDestinationList { get; set; }
    }
}