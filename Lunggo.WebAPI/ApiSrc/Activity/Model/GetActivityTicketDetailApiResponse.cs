using Lunggo.ApCommon.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetActivityTicketDetailApiResponse : ApiResponseBase
    {
        [JsonProperty("packageTicketPrice", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityPackage> PackageTicketPrice { get; set; }
    }
}