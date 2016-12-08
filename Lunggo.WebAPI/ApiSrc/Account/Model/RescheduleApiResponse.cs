using System;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Account.Model
{
    public class RescheduleApiResponse : ApiResponseBase
    {
        [JsonProperty("isSuccess", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsSuccess { get; set; }
    }
}