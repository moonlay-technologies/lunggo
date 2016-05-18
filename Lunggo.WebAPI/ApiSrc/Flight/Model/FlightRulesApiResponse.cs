using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightRulesApiResponse : ApiResponseBase
    {
        [JsonProperty("airlines")]
        public List<AirlineRules> AirlineRules { get; set; }
        [JsonProperty("baggages")]
        public List<BaggageRules> BaggageRules { get; set; }
    }
}