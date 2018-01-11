using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class LandingPageApiRequest
    {
        [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
        public string Contact;
    }
}