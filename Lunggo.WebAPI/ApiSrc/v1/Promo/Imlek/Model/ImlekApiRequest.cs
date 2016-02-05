using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Model
{
    public class ImlekApiRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}