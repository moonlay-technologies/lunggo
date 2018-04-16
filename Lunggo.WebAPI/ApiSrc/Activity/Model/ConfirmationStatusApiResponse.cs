using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ConfirmationStatusApiResponse : ApiResponseBase
    {
        [JsonProperty("isValid", NullValueHandling = NullValueHandling.Ignore)]
        public bool isValid { get; set; }
    }
}