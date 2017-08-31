using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Product.Constant;
using Lunggo.ApCommon.Product.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class GetSavedPassengerResponse : ApiResponseBase
    {
        [JsonProperty("paxList", NullValueHandling = NullValueHandling.Ignore)]
        public List<PaxForDisplay> PaxList {get; set; }
    }
}