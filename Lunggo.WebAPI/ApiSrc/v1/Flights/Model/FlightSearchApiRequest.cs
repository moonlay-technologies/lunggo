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
        public string SearchId { get; set; }
        [DataMember(Name = "requests")]
        public List<int> Requests { get; set; }
    }
}