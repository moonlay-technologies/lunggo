using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetActivityTicketDetailApiRequest : ApiRequestBase
    {
        [JsonProperty("activityId", NullValueHandling = NullValueHandling.Ignore)]
        public string ActivityId { get; set; }
    }
}