using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GenerateQuestionApiResponse : ApiResponseBase
    {
        [JsonProperty("questions", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Questions { get; set; }
    }
}