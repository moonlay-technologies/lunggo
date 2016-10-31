using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelSelectRoomApiResponse : ApiResponseBase
    {
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token{ get; set; }
        [JsonProperty("timeLimit", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? TimeLimit { get; set; }

   //     public PaxReqirement Type { get; set; }
    }
}