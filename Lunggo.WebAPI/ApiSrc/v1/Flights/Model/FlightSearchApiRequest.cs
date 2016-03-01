using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Helpers;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightSearchApiRequest
    {
        [JsonProperty("search_id")]
        public string SearchId { get; set; }
        [JsonProperty("requests")]
        public List<int> Requests { get; set; }
    }
}