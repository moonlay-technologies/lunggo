using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetReservationListApiRequest
    {
        [JsonProperty("page")]
        public string Page { get; set; }
        [JsonProperty("perPage")]
        public string PerPage { get; set; }
    }
}