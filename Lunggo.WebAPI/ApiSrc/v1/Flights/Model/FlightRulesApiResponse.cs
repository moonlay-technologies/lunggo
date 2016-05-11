using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRulesApiResponse : ApiResponseBase
    {
        [JsonProperty("airlines")]
        public List<AirlineRules> AirlineRules { get; set; }
        [JsonProperty("baggages")]
        public List<BaggageRules> BaggageRules { get; set; }
    }
}