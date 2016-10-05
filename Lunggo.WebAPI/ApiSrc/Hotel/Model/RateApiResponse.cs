using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class RateApiResponse
    {
        [JsonProperty("rates")]
        public List<Rate> Rates{ get; set; } 
    }
}