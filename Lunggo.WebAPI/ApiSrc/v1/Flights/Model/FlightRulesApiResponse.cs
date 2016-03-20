using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRulesApiResponse : ApiResponseBase
    {
        [JsonProperty("a")]
        public List<AirlineRules> AirlineRules { get; set; }
        [JsonProperty("b")]
        public List<BaggageRules> BaggageRules { get; set; }
    }
}